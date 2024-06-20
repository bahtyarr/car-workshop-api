namespace CarWorkshopSystem.Core.Enums
{
    public enum GeneralProcessStatusType
    {
        Waiting,
        Progress,
        Completed,
    }

    public class GeneralProcessStatusTypeList
    {
        public static Dictionary<string, string> Map()
        {
            var result = new Dictionary<string, string>
        {
            { GeneralProcessStatusType.Waiting.ToString(), GeneralProcessStatusType.Waiting.ToString()},
            { GeneralProcessStatusType.Progress.ToString(), GeneralProcessStatusType.Progress.ToString() },
            { GeneralProcessStatusType.Completed.ToString(), GeneralProcessStatusType.Completed.ToString() },
        };

            return result;
        }
    }
}