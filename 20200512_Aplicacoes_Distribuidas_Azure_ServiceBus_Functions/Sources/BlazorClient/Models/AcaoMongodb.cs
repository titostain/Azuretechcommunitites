using System;

namespace ClientAcoes.Models
{
    public class AcaoMongodb
    {
        public string Codigo { get; set; }
        public decimal Valor { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
    }
}