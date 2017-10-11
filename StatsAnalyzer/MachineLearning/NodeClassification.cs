using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer.MachineLearning
{
    internal class NodeClassification
    {
        private String _name;
        private int _numMalwareClassifications = 0;
        private int _numGoodwareClassifications = 0;
        private int _actualClass;
        private int _correctPredictions = 0;
        private List<double> _rawPredictions;
        

        internal NodeClassification(String name, int actualClass)
        {
            this._name = name;
            this._rawPredictions = new List<double>();
        }

        internal NodeClassification(String name, double initialPrediction, int actualClass)
        {
            this._name = name;
            this._rawPredictions = new List<double>();
            this._actualClass = actualClass;
            this.addPrediction(initialPrediction);

            if (name.ToLower().Contains("malware") && actualClass < 0)
                Console.WriteLine("Warning: Possible classification error - node <" + name + "> is classified with " + actualClass);
            else if (name.ToLower().Contains("goodware") && actualClass > 0)
                Console.WriteLine("Warning: Possible classification error - node <" + name + "> is classified with " + actualClass);
        }

        internal void addPrediction(double prediction)
        {
            if (prediction < 0)
                _numGoodwareClassifications++;
            else
                _numMalwareClassifications++;

            _rawPredictions.Add(prediction);

            if (Math.Sign(prediction) == this._actualClass)
                _correctPredictions++;
        }

        internal int NumMalwarePredictions
        {
            get { return this._numMalwareClassifications; }
        }

        internal int NumGoodwarePredictions
        {
            get { return this._numGoodwareClassifications; }
        }

        internal List<double> RawPredictions
        {
            get { return new List<double>(this._rawPredictions); }
        }

        internal int ActualClass
        {
            get { return this._actualClass; }
        }

        internal int TotalPredictions
        {
            get { return this._rawPredictions.Count; }
        }

        internal int CorrectPredictions
        {
            get { return this._correctPredictions; }
        }

        internal int FalsePredictions
        {
            get { return this.TotalPredictions - this.CorrectPredictions; }
        }

    }
}
