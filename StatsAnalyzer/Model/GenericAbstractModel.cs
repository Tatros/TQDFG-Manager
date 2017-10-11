using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace StatsAnalyzer.Model
{
    internal abstract class GenericAbstractModel : IModel
    {
        protected Analyzer _analyzer;
        protected List<GenericNode> _nodes;
        protected List<String> _featureNames;
        protected double _missingValue;
        protected String _numberFormat;
        protected String _separator;
        protected String _missingValueIdentifier;
        protected int _numSamples = -1;
        protected MODEL _type;
        //protected DataTable _dataTable;
        public event EventHandler ModelChanged;


        public void saveToFile(String path)
        {
            System.IO.File.WriteAllText(path, this.ToString());
        }

        public virtual DataTable DataTable
        {
            get
            {
                DataTable table = new DataTable();
                table.Columns.Add("Name",typeof(String));
                table.Columns.Add("NodeID",typeof(double));

                _nodes[0].getFeatureNames().ForEach(feature =>
                {
                    table.Columns.Add(feature);
                });

                _nodes.ForEach(node =>
                {
                    List<object> values = new List<object> { node.Name, node.ID };
                    values.AddRange(node.getFeatureValueArray().Cast<object>().ToList());
                    table.Rows.Add(values.ToArray());
                    //Utility.writeToConsole<double>(values);
                });

                return table;
            }
        }

        public virtual DataTable FeatureTable
        {
            get
            {
                DataTable table = new DataTable();

                _nodes[0].getFeatureNames().ForEach(feature =>
                {
                    table.Columns.Add(feature);
                });

                _nodes.ForEach(node =>
                {
                    List<object> values = new List<object>();
                    values.AddRange(node.getFeatureValueArray().Cast<object>().ToList());
                    table.Rows.Add(values.ToArray());
                    //Utility.writeToConsole<double>(values);
                });

                return table;
            }
        }

        public virtual DataTable ClassifiedFeatureTable
        {
            get
            {
                DataTable table = new DataTable();

                _nodes[0].getFeatureNames().ForEach(feature =>
                {
                    table.Columns.Add(feature);
                });
                table.Columns.Add("class");

                _nodes.ForEach(node =>
                {
                    if (node.isMalware() || node.isGoodware())
                    {
                        List<object> values = new List<object>();
                        values.AddRange(node.getFeatureValueArray().Cast<object>().ToList());

                        if (node.isGoodware())
                            values.Add((double)-1.0);
                        else
                            values.Add((double)1.0);

                        table.Rows.Add(values.ToArray());
                        //Utility.writeToConsole<double>(values);
                    }
                });

                return table;
            }
        }

        public List<String> getFeatureNames(String nodeName)
        {
            List<GenericNode> matchingNodes = (_nodes.Where(n => n.Name == nodeName)).ToList();

            if (matchingNodes.Count >= 1)
                return matchingNodes[0].getFeatureNames();
            else
                throw new ArgumentException("No node found with name <" + nodeName + ">");
        }

        public double getFeatureValue(String nodeName, String featureName)
        {
            List<GenericNode> matchingNodes = (_nodes.Where(n => n.Name == nodeName)).ToList();

            if (matchingNodes.Count >= 1)
                return matchingNodes[0].getFeatureValue(featureName);
            else
                throw new ArgumentException("No node found with name <" + nodeName + ">");
        }

        public virtual MODEL Type
        {
            get
            {
                return _type;
            }
        }

        public virtual String NumberFormat
        {
            get { return _numberFormat; }
            set { _numberFormat = value; ModelChanged.Invoke(this, new EventArgs()); }
        }

        public virtual String Separator
        {
            get { return _separator; }
            set { _separator = value; ModelChanged.Invoke(this, new EventArgs()); }
        }

        public virtual Double MissingValue
        {
            get { return _missingValue; }
            set { this._missingValue = value; ModelChanged.Invoke(this, new EventArgs()); }
        }

        public virtual String MissingValueIdentifier
        {
            get { return _missingValueIdentifier; }
            set { _missingValueIdentifier = value; ModelChanged.Invoke(this, new EventArgs()); }
        }

        public virtual int NumSamples
        {
            get { return _numSamples; }
            set { _numSamples = value; ModelChanged.Invoke(this, new EventArgs()); }
        }

        protected virtual bool isValidSampleSet(List<Double> sampleSet)
        {
            return true;
        }

        protected void raiseModelChanged()
        {
            ModelChanged.Invoke(this, new EventArgs());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            // start with header
            sb.Append("Name");
            _featureNames.ForEach(featureName => sb.Append(_separator + featureName));
            sb.AppendLine(_separator + "Class");

            foreach (GenericNode node in _nodes)
            {
                sb.Append(node.Name);

                _featureNames.ForEach(featureName =>
                {
                    double value = node.getFeatureValue(featureName);
                    if (!Double.IsNaN(value) && !Double.IsInfinity(value) && value != _missingValue)
                    {
                        //DDEBUG: sb.Append(_separator + featureName + ":" + value.ToString(_numberFormat));
                        sb.Append(_separator + value.ToString(_numberFormat));
                    }
                    else
                    {
                        //DEBUG: sb.Append(_separator + featureName + ":" + _missingValueIdentifier);
                        sb.Append(_separator + _missingValueIdentifier);
                    }
                });

                // Add the classifier
                Analyzer.NodeType nodeType = _analyzer.getNodeType(node.Name);
                if (nodeType == Analyzer.NodeType.GOODWARE)
                    sb.AppendLine(_separator + "goodware");
                else if (nodeType == Analyzer.NodeType.MALWARE)
                    sb.AppendLine(_separator + "malware");
                else
                    sb.AppendLine(_separator + "unknown");
            }

            return sb.ToString();
        }

        public virtual INode getNode(String nodeName)
        {
            List<GenericNode> matchingNodes = (_nodes.Where(n => n.Name == nodeName)).ToList();

            if (matchingNodes.Count >= 1)
                return matchingNodes[0];
            else
                throw new ArgumentException("No node found with name <" + nodeName + ">");
        }

        public virtual List<INode> getNodes()
        {
            return _nodes.ToList<INode>();
        }

        public virtual List<String> getFeatureNames()
        {
            return _nodes.First().getFeatureNames();
        }

        public virtual Model.NodeType getNodeType(String nodeName)
        {
            if (nodeName.ToLower().Contains("malware"))
                return NodeType.MALWARE;

            else if (nodeName.ToLower().Contains("goodware"))
                return NodeType.GOODWARE;

            else
                return NodeType.UNKNOWN;
        }

        public virtual String Name
        {
            get { return "Generic"; }
        }

        protected virtual double getDecisionSample(StaticModel.DECISION_SAMPLE decision, List<double> featureValues)
        {
            switch (decision)
            {
                case StaticModel.DECISION_SAMPLE.FIRST_VALUE:
                    {
                        return featureValues.First();
                    }
                case StaticModel.DECISION_SAMPLE.LAST_VALUE:
                    {
                        return featureValues.Last();
                    }
                case StaticModel.DECISION_SAMPLE.MEDIAN_INDEX:
                    {
                        int medianIndex = (int)Math.Round((double)featureValues.Count * 0.5, 0);
                        return featureValues[medianIndex];
                    }
                case StaticModel.DECISION_SAMPLE.MEDIAN_VALUE:
                    {
                        if (featureValues.Count > 2)
                        {
                            List<Double> sortedValues = new List<Double>(featureValues);
                            sortedValues.Sort();
                            int size = sortedValues.Count;
                            int middleIndex = size / 2;
                            double medianValue = (size % 2 != 0) ? (double)sortedValues[middleIndex] : ((double)sortedValues[middleIndex] + (double)sortedValues[middleIndex - 1]) / 2;
                            return medianValue;
                        }

                        else
                        {
                            return featureValues.First();
                        }
                    }
                default:
                    {
                        throw new NotImplementedException("Static feature extraction for decision sample <" + decision.ToString() + "> is not yet implemented.");
                    }
            }
        }
    }
}
