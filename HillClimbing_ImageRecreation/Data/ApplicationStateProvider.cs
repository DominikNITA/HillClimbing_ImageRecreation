using Logic;
using Logic.Models;

namespace HillClimbing_ImageRecreation.Data
{
    public class ApplicationStateProvider
    {
        public ApplicationStateProvider()
        {
            ResetState();
        }

        public void ResetState()
        {
            State = ApplicationState.SelectingImage;
            AnalysisResult = null;
            Parameters = null;
            Algorithm = null;
            AlgorithmResults = new();
        }

        private ApplicationState _state;

        public ApplicationState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    NotifyStateChanged();
                }
            }
        }

        public event Action? OnStateChange;

        private void NotifyStateChanged() => OnStateChange?.Invoke();



        private AlgorithmParameters _parameters;

        public AlgorithmParameters Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
                NotifyParametersChanged();
            }
        }

        public event Action? OnParametersChange;

        private void NotifyParametersChanged() => OnParametersChange?.Invoke();



        public Algorithm Algorithm { get; set; }

        public AnalysisResult AnalysisResult { get; set; }

        private List<AlgorithmResult> _algorithmResults = new();
        public List<AlgorithmResult> AlgorithmResults
        {
            get { return _algorithmResults; }
            private set { _algorithmResults = value; }
        }

        public void AddAlgorithmResult(AlgorithmResult algorithmResult)
        {
            _algorithmResults.Add(algorithmResult);
            NotifyAlgorithmResultAdded();
        }

        public event Action? OnAlgorithmResultAdd;
        private void NotifyAlgorithmResultAdded() => OnAlgorithmResultAdd?.Invoke();

    }
}
