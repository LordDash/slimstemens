using System;

[Serializable]
public class OpenDoorQuestion : Question
{
    public string Question;
    public string QuestionFileName;
    public OpenDoorAnswer[] Answers;
}