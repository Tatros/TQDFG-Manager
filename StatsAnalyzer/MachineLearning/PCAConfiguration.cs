using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Analysis;

namespace StatsAnalyzer.MachineLearning
{
    internal class PCAConfiguration
    {
        private AnalysisMethod _method = AnalysisMethod.Center;
        private String _pcaExportSeparator = ";";
        private String _pcaExportMissingValueIdentifier = "?";
        private String _pcaExportNumberFormat = "0.00000";
        private String _pcaExportForcedLabel = "";
        private int _pcaExportForcedNumComponents = -1;
        private bool _pcaExportUseForcedLabel = false;
        private bool _pcaExportUseForcedNumComponents = false;
        private List<String> _activeFeatures;

        public PCAConfiguration(List<String> activeFeatures = null) 
        {
            ActiveFeatures = activeFeatures;
        }
        internal AnalysisMethod Method
        {
            get { return _method; }
            set { this._method = value; }
        }

        internal String PCAExportSeparator
        {
            get { return _pcaExportSeparator; }
            set { _pcaExportSeparator = value; }
        }

        internal String PCAExportMissingValueIdentifier
        {
            get { return _pcaExportMissingValueIdentifier; }
            set { _pcaExportMissingValueIdentifier = value; }
        }
        internal String PCAExportNumberFormat
        {
            get { return _pcaExportNumberFormat; }
            set { _pcaExportNumberFormat = value; }
        }

        internal String PCAExportForcedLabel
        {
            get { return _pcaExportForcedLabel; }
            set { _pcaExportForcedLabel = value; }
        }

        internal int PCAExportForcedNumComponents
        {
            get { return _pcaExportForcedNumComponents; }
            set { _pcaExportForcedNumComponents = value; }
        }

        internal bool PCAExportUseForcedNumComponents
        {
            get { return _pcaExportUseForcedNumComponents; }
            set { _pcaExportUseForcedNumComponents = value; }
        }

        internal bool PCAExportUseForcedLabel
        {
            get { return _pcaExportUseForcedLabel; }
            set { _pcaExportUseForcedLabel = value; }
        }

        internal List<String> getAvailableAnalysisMethods()
        {
            return Enum.GetNames(typeof(AnalysisMethod)).ToList();
        }

        internal List<String> ActiveFeatures
        {
            get 
            {
                if (_activeFeatures == null)
                    _activeFeatures = new List<string>();

                return _activeFeatures;
            }
            set { this._activeFeatures = value; }
        }
    }
}
