using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;

public class GameEditor : EditorWindow
{
    [MenuItem("Assets/Create/Game")]
    static void CreateGame()
    {
        Game game = new Game();

        string serializedGame = JsonUtility.ToJson(game);

        string assetPath;

        if (Selection.activeObject == null)
        {
            assetPath = "Assets/New Game.txt";
        }
        else
        {
            string activeObjectPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (Directory.Exists(activeObjectPath))
            {
                assetPath = activeObjectPath + "/New Game.txt";
            }
            else
            {
                assetPath = Path.GetDirectoryName(activeObjectPath) + "/New Game.txt";
            }
        }

        File.WriteAllText(assetPath, serializedGame);

        AssetDatabase.Refresh();
        TextAsset file = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
        Selection.activeObject = file;
    }

    [MenuItem("Assets/Edit/Game ...")]
    static void EditGame()
    {
        TextAsset file = Selection.activeObject as TextAsset;

        if (file != null)
        {
            GameEditor window = EditorWindow.GetWindow<GameEditor>("Game Editor");
            window.SetFile(file);
            window.Show();
        }
    }

    private TextAsset _file;
    private string _currentFileTitle = "Please set a game file to edit.";
    private string _error;
    private Game _game;

    private bool _dirty = false;

    private Round _roundToAdd = default(Round);
    private int _roundIndexToRemove = -1;
    private Dictionary<int, bool> _roundCollapsed = new Dictionary<int, bool>();
    private Dictionary<int, Dictionary<int, bool>> _questionCollapsed = new Dictionary<int, Dictionary<int, bool>>();
    private Vector2 _scrollPosition;

    private void SetFile(TextAsset file)
    {
        _file = file;
        _dirty = false;

        if (_file != null)
        {
            string assetDirectoryPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(_file));

            _currentFileTitle = string.Format("{0} [{1}]", _file.name, assetDirectoryPath);

            try
            {
                _game = JsonUtility.FromJson<Game>(_file.text);
            }
            catch(ArgumentException)
            {
                _game = null;
            }

            if (_game == null)
            {
                _error = string.Format("{0} is not a game. Please select a game file.", _file.name);
            }
            else
            {
                _error = "";
            }
        }
        else
        {
            _currentFileTitle = "Please set a game file to edit.";
            _error = "";
        }
    }

    private void SetGameDirty()
    {
        if (_dirty == false)
        {
            string assetDirectoryPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(_file));
            _currentFileTitle = string.Format("{0}* [{1}]", _file.name, assetDirectoryPath);

            _dirty = true;
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(_currentFileTitle, EditorStyles.boldLabel);

            if (GUILayout.Button("Reload"))
            {
                SetFile(_file);
            }

            if (GUILayout.Button("Save"))
            {
                if (_dirty)
                {
                    string assetPath = AssetDatabase.GetAssetPath(_file);
                    File.WriteAllText(assetPath, _game.Save());
                    AssetDatabase.Refresh();

                    SetFile(_file);
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        TextAsset newFile = EditorGUILayout.ObjectField("File", _file, typeof(TextAsset), false) as TextAsset;

        if (newFile != _file)
        {
            SetFile(newFile);
        }

        if (string.IsNullOrEmpty(_error) == false)
        {
            EditorGUILayout.HelpBox(_error, MessageType.Error);
        }

        if (_game == null)
        {
            return;
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Delete All"))
            {
                if (EditorUtility.DisplayDialog("Delete all round", "Are you sure you want to delete all rounds?", "Delete", "Cancel"))
                {
                    _game.GameRounds = new List<Round>();
                    _game.SerializedRoundQuestions = new List<string>();

                    SetGameDirty();
                }
            }

            if (GUILayout.Button("Collapse All"))
            {
                for (int i = 0; i < _game.GameRounds.Count; i++)
                {
                    _roundCollapsed[i] = true;

                    Question[] questions = _game.GetQuestionsForRound<Question>(i);

                    _questionCollapsed[i] = new Dictionary<int, bool>();

                    for (int j = 0; j < questions.Length; j++)
                    {
                        _questionCollapsed[i][j] = true;
                    }
                }
            }

            if (GUILayout.Button("Open All"))
            {
                for (int i = 0; i < _game.GameRounds.Count; i++)
                {
                    _roundCollapsed[i] = false;

                    Question[] questions = _game.GetQuestionsForRound<Question>(i);

                    _questionCollapsed[i] = new Dictionary<int, bool>();

                    for (int j = 0; j < questions.Length; j++)
                    {
                        _questionCollapsed[i][j] = false;
                    }
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true);

            for (int i = 0; i < _game.GameRounds.Count; i++)
            {
                switch (_game.GetRound(i))
                {
                    case Round.ThreeSixNine:
                        DrawThreeSixNine(i);
                        break;
                    case Round.Puzzle:
                        DrawPuzzle(i);
                        break;
                }
            }

            EditorGUILayout.EndScrollView();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        {
            _roundToAdd = (Round)EditorGUILayout.EnumPopup(_roundToAdd);

            if (GUILayout.Button("Add Round"))
            {
                SetGameDirty();

                switch (_roundToAdd)
                {
                    case Round.ThreeSixNine:
                        ThreeSixNineQuestion[] threeSixNineQuestions = new ThreeSixNineQuestion[15];
                        for(int i = 0; i < threeSixNineQuestions.Length; i++)
                        {
                            threeSixNineQuestions[i] = new ThreeSixNineQuestion();
                        }

                        QuestionContainer<ThreeSixNineQuestion> threeSixNineContainer = new QuestionContainer<ThreeSixNineQuestion>();
                        threeSixNineContainer.Questions = threeSixNineQuestions;

                        _game.AddRound(_roundToAdd, threeSixNineContainer);
                        break;
                    case Round.Puzzle:
                        PuzzleQuestion[] puzzleQuestions = new PuzzleQuestion[3];
                        for (int i = 0; i < puzzleQuestions.Length; i++)
                        {
                            puzzleQuestions[i] = new PuzzleQuestion();
                            puzzleQuestions[i].Answers = new PuzzleAnswer[3];

                            for (int j = 0; j < puzzleQuestions[i].Answers.Length; j++)
                            {
                                puzzleQuestions[i].Answers[j] = new PuzzleAnswer();
                                puzzleQuestions[i].Answers[j].Words = new[] { "", "", "", "" };
                            }
                        }

                        QuestionContainer<PuzzleQuestion> puzzleContainer = new QuestionContainer<PuzzleQuestion>();
                        puzzleContainer.Questions = puzzleQuestions;

                        _game.AddRound(_roundToAdd, puzzleContainer);
                        break;
                }

                
            }
        }
        EditorGUILayout.EndHorizontal();

        if (_roundIndexToRemove != -1)
        {
            _game.RemoveRound(_roundIndexToRemove);
            SetGameDirty();
            _roundIndexToRemove = -1;
        }
    }

    private void DrawPuzzle(int roundIndex)
    {
        bool collapsed;
        _roundCollapsed.TryGetValue(roundIndex, out collapsed);

        PuzzleQuestion[] questions = _game.GetQuestionsForRound<PuzzleQuestion>(roundIndex);

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("x", GUILayout.Width(20)))
            {
                if (EditorUtility.DisplayDialog("Delete round", "Are you sure you want to delete this round?", "Delete", "Cancel"))
                {
                    _roundIndexToRemove = roundIndex;
                }
            }

            if (GUILayout.Button(collapsed ? "▼" : "▲", GUILayout.Width(20)))
            {
                _roundCollapsed[roundIndex] = !collapsed;
            }

            EditorGUILayout.LabelField("Puzzle" + (collapsed ? string.Format(" ({0})", questions.Length) : ""), EditorStyles.boldLabel);
        }
        EditorGUILayout.EndHorizontal();

        if (collapsed)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("", GUILayout.Width(40));

            EditorGUILayout.BeginVertical();
            {
                for (int i = 0; i < questions.Length; i++)
                {
                    bool questionCollapsed;
                    if (_questionCollapsed.ContainsKey(roundIndex) == false)
                    {
                        _questionCollapsed[roundIndex] = new Dictionary<int, bool>();
                    }
                    _questionCollapsed[roundIndex].TryGetValue(i, out questionCollapsed);

                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button(questionCollapsed ? "▼" : "▲", GUILayout.Width(20)))
                        {
                            _questionCollapsed[roundIndex][i] = !questionCollapsed;
                        }

                        EditorGUILayout.LabelField("Puzzle " + (i + 1));
                    }
                    EditorGUILayout.EndHorizontal();

                    if (questionCollapsed)
                    {
                        continue;
                    }


                    EditorGUI.BeginChangeCheck();

                    for (int j = 0; j < questions[i].Answers.Length; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.LabelField("Answer");
                            EditorGUILayout.LabelField("Words");
                            EditorGUILayout.LabelField("Time Reward", GUILayout.Width(80));
                        }
                        EditorGUILayout.EndHorizontal();

                        for (int k = 0; k < questions[i].Answers[j].Words.Length; k++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                if (k == 0)
                                {
                                    questions[i].Answers[j].Answer = EditorGUILayout.TextField(questions[i].Answers[j].Answer);
                                    questions[i].Answers[j].Words[k] = EditorGUILayout.TextField(questions[i].Answers[j].Words[k]);
                                    questions[i].Answers[j].TimeReward = EditorGUILayout.IntField(questions[i].Answers[j].TimeReward, GUILayout.Width(80));
                                }
                                else
                                {
                                    EditorGUILayout.LabelField("");
                                    questions[i].Answers[j].Words[k] = EditorGUILayout.TextField(questions[i].Answers[j].Words[k]);
                                    EditorGUILayout.LabelField("", GUILayout.Width(80));
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        SetGameDirty();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }
    
    private void DrawThreeSixNine(int roundIndex)
    {
        bool collapsed;
        _roundCollapsed.TryGetValue(roundIndex, out collapsed);

        ThreeSixNineQuestion[] questions = _game.GetQuestionsForRound<ThreeSixNineQuestion>(roundIndex);

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("x", GUILayout.Width(20)))
            {
                if (EditorUtility.DisplayDialog("Delete round", "Are you sure you want to delete this round?", "Delete", "Cancel"))
                {
                    _roundIndexToRemove = roundIndex;
                }
            }

            if (GUILayout.Button(collapsed ? "▼" : "▲", GUILayout.Width(20)))
            {
                _roundCollapsed[roundIndex] = !collapsed;
            }

            EditorGUILayout.LabelField("Three Six Nine" + (collapsed ? string.Format(" ({0})", questions.Length) : ""), EditorStyles.boldLabel);
        }
        EditorGUILayout.EndHorizontal();

        if (collapsed)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Question");
                    EditorGUILayout.LabelField("Answer");
                    EditorGUILayout.LabelField("Time Reward", GUILayout.Width(80));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();

                for (int i = 0; i < questions.Length; i++)
                {
                    ThreeSixNineQuestion question = questions[i];

                    EditorGUILayout.BeginHorizontal();
                    {
                        question.Question = EditorGUILayout.TextField(question.Question);
                        question.Answer = EditorGUILayout.TextField(question.Answer);
                        question.TimeReward = EditorGUILayout.IntField(question.TimeReward, GUILayout.Width(80));
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (EditorGUI.EndChangeCheck())
                {
                    SetGameDirty();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }
}