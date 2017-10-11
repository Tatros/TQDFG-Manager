using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsAnalyzer.Model
{
    // public enum MODEL { STATIC, DYNAMIC, STATISTICAL, STATISTICAL_FILL, STATISTICAL_COUNT, STATISTICAL_FILL_COUNT, FFT, POLY_FIT_1, POLY_FIT_2, POLY_FIT_3, POLY_FIT_4, POLY_FIT_5, POLY_FIT_6, POLY_FIT_7, POLY_FIT_8 };
    public enum MODEL { STATIC, SAMPLED, STATISTICAL, POLY_FIT };
    public enum NodeType { MALWARE, GOODWARE, UNKNOWN };
}
