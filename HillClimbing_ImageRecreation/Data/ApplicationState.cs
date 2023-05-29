using System.ComponentModel;

namespace HillClimbing_ImageRecreation.Data
{
    public enum ApplicationState
    {
        [Description("Image selection")]
        SelectingImage,
        [Description("Analyzing image")]
        AnalyzingImage,
        [Description("Select parameters")]
        SettingParameters,
        [Description("Generating image")]
        AlgorithmWorking,
        [Description("Generation stopped")]
        AlgorithmStopped,
        [Description("Generation finished")]
        AlgorithmFinished,
        [Description("Video generated")]
        TimelapseGenerated,
        [Description("Generating timelapse video")]
        GeneratingTimelapse,
    }
}
