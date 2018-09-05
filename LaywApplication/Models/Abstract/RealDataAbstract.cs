using Newtonsoft.Json;
using System;

namespace LaywApplication.Models.Abstract
{
    public abstract class RealDataAbstract<TType>
    {
        public DateTime Date { get; set; }

        [JsonIgnore]
        public TType RealData { get; set; }
    }
}
