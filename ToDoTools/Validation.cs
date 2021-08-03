namespace ToDoTools
{
    public static class Validation
    {
       public static bool AtLeast8Characters(string value)
        {
            if (value.Length < 8) return true;
            return false;
        }

        public static bool CanNotBeEmpty(string value)
        {
            if (value == string.Empty) return true;
            return false;
        }
    }
}
