
namespace DHOG_WPF.Models
{
    public class ProblemConfigurationParameter : BasicEntity
    {
        public ProblemConfigurationParameter(string name, int value, string uiName)
        {
            Name = name;
            Value = value;
            UIName = uiName;
        }

        public string UIName { get; set; }
        public int Value { get; set; }
    }
}
