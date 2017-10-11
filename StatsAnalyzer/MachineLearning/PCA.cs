using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord;
using Accord.MachineLearning;
using Accord.Statistics.Analysis;
using StatsAnalyzer.Model;
using System.Data;
using Accord.Math;
using System.IO;

namespace StatsAnalyzer.MachineLearning
{
    internal class PCA
    {
        private double[,] _data;
        private double[,] _transformedData;
        private List<INode> _nodes;
        private IModel _model;
        private PrincipalComponentAnalysis _pca;
        private string[] _columnNames;
        private DataTable _modelData;
        private PCAConfiguration _configuration;


        internal PCA(IModel model, List<String> nodeNames, PCAConfiguration configuration)
        {
            this._model = model;
            this._configuration = configuration;
            loadData(nodeNames);
        }

        private void loadData(List<String> nodeNames)
        {
            _modelData = _model.DataTable;
            this._nodes = _model.getNodes();
            //_columnNames = _nodes.First().getFeatureNames().ToArray();
            //_columnNames = new string[] { "NodeID","inSizeRatioProcessNode", "inSizeRatioFileNode", "inSizeRatioSocketNode", "outSizeRatioProcessNode", "outSizeRatioFileNode", "outSizeRatioSocketNode", "inEdgeCompression", "outEdgeCompression", "sizeStdDev", "sizeEnt", "boundedPageRankNormalized", "boundClosenessSizeCentralityNormalized", "boundBetweennessSizeCentrality", "pageRankNormalized", "closenessSizeCentralityNormalized", "betweennessSizeCentrality" };
            _columnNames = _configuration.ActiveFeatures.ToArray();

            _data = _modelData.ToMatrix<double>(_columnNames);
        }

        internal Double[,] Data
        {
            get { return this._data; }
        }

        internal Double[,] TransformedData
        {
            get { return this._transformedData; }
        }

        internal DataTable ModelData
        {
            get { return this._modelData; }
        }

        internal PrincipalComponentAnalysis PrincipalComponentAnalysis
        {
            get { return this._pca; }
        }

        internal void compute()
        {
            // Create and compute a new Simple Descriptive Analysis
            var sda = new DescriptiveAnalysis(_data, _columnNames);
            sda.Compute();

            // Create the Principal Component Analysis of the data 
            _pca = new PrincipalComponentAnalysis(sda.Source, _configuration.Method);
            _pca.Compute();

            if (_configuration.PCAExportUseForcedNumComponents && 
                (_pca.Components.Count > _configuration.PCAExportForcedNumComponents))
            { 
                _transformedData = _pca.Transform(_data, _configuration.PCAExportForcedNumComponents);
            }

            else
            {
                _transformedData = _pca.Transform(_data, _pca.Components.Count);
            }
        }

        internal List<String> ColumnHeaders
        {
            get { return this._columnNames.ToList(); }
        }

        internal List<INode> Nodes
        {
            get { return this._nodes; }
        }

        internal void export(String path)
        {
            File.WriteAllText(path, this.ToString());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            List<String> featureNames = _columnNames.ToList();
            // start with header
            sb.Append("Name");
            if (featureNames.Count == _transformedData.GetLength(1))
                featureNames.ForEach(featureName => sb.Append(_configuration.PCAExportSeparator + featureName));
            else
                for (int i = 0; i < _transformedData.GetLength(1); i++)
                    sb.Append(_configuration.PCAExportSeparator + "feature_" + i);
            
            sb.AppendLine(_configuration.PCAExportSeparator + "Class");

            for (int i = 0; i < _transformedData.GetLength(0); i++)
            {
                sb.Append(_nodes[i].Name);
                for (int j = 0; j < _transformedData.GetLength(1); j++)
                {
                    double value = _transformedData[i,j];

                    if (!Double.IsNaN(value) && !Double.IsInfinity(value))
                    {
                        sb.Append(_configuration.PCAExportSeparator + value.ToString(_configuration.PCAExportNumberFormat));
                    }
                    else
                    {
                        sb.Append(_configuration.PCAExportSeparator + _configuration.PCAExportMissingValueIdentifier);
                    }
                }

                // Add the classifier
                Model.NodeType nodeType = _model.getNodeType(_nodes[i].Name);
                if (nodeType == Model.NodeType.GOODWARE)
                    sb.AppendLine(_configuration.PCAExportSeparator + "goodware");
                else if (nodeType == Model.NodeType.MALWARE)
                    sb.AppendLine(_configuration.PCAExportSeparator + "malware");
                else
                    sb.AppendLine(_configuration.PCAExportSeparator + "unknown");
            }

            return sb.ToString();
        }
    }
}
