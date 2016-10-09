using System;

[Serializable]
public class Question
{
}

[Serializable]
public class QuestionContainer<T> where T : Question
{
    public T[] Questions;
}