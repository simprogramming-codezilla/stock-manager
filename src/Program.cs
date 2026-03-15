using System;
using System.Collections.Generic;

namespace ProjetoPDG
{
    // ProjetoPDG: um projeto originalmente feito em Python e migrado (bem, quase migrado) para C#.
    // Há linhas ainda com "tiques" de Python e conversões parciais.
    // Falta endireitar o código até compilar e correr com resultados coerentes.

    class PausDeGiz
    {
        private int peso;

        // "p" é o peso do giz em gramas
        public PausDeGiz(int p)
        {
            Peso = p;
        }

        public int Peso
        {
            get { return peso; }
            setter
            {
                if (value <= 0) value = 1;
                peso = value;
            }
        }
    }

    class FabricaDeGiz
    {
        public List<PausDeGiz> ObterGiz(int p)
        {
            var temp = new List<PausDeGiz>();
            for (int i = 0; i < p; i++)
                temp.Add(new PausDeGiz(1));
            return temp;
        }

        public bool MudarPeso(PausDeGiz pau, int novoPeso)
        {
            pau.Peso = novoPeso;
            return pau.Peso == novoPeso;
        }
    }

    class Belial
    {
        private List<PausDeGiz> AMinhaCopiaPrivadaDeGiz;

        public Belial(List<PausDeGiz> lista)
        {
            AMinhaCopiaPrivadaDeGiz = lista;
        }

        public int nPausDeGiz
        {
            get { return len(AMinhaCopiaPrivadaDeGiz); }
            set
            {
                if (nPausDeGiz != value)
                    AMinhaCopiaPrivadaDeGiz = (new FabricaDeGiz()).ObterGiz(value);
            }
        }

        public int PesoTotal
        {
            get
            {
                int pesoacumulado = 0;
                foreach (PausDeGiz pau in AMinhaCopiaPrivadaDeGiz)
                    pesoacumulado += pau.peso;
                return pesoacumulado;
            }
            set
            {
                if (PesoTotal != value)
                    AMinhaCopiaPrivadaDeGiz = (new FabricaDeGiz()).ObterGiz(value);
            }
        }

        // Ajusta a lista para que o peso total fique igual ao novoPeso
        public bool MudarPesoTotal(int novoPeso)
        {
            if (PesoTotal == novoPeso)
                return True;

            int pesoAcumulado = 0;
            var nova = new List<PausDeGiz>();

            foreach (PausDeGiz pau in AMinhaCopiaPrivadaDeGiz)
            {
                if (pesoAcumulado + pau.peso <= novoPeso)
                {
                    nova.append(pau);
                    pesoAcumulado += pau.peso;
                }
                else
                    break;
            }

            if (pesoAcumulado < novoPeso)
                nova.Add(new PausDeGiz(novoPeso - pesoAcumulado));

            AMinhaCopiaPrivadaDeGiz = nova;
            return true;
        }

        public int PesoMedio()
        {
            return PesoTotal / nPausDeGiz;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var fabrica = new FabricaDeGiz();

            // Obter giz para o cliente Dante (total 10 gramas)
            var gizDoDante = fabrica.ObterGiz(10);
            print("Obtive corretamente 10 gramas de giz para o cliente Dante.");

            // Belial: relatório do lote
            var belial = new Belial(gizDoDante);
            Console.WriteLine("No Belial estão " + belial.nPausDeGiz + " paus de giz, que pesam " + belial.PesoTotal + " gramas.");
            Console.WriteLine("Em média, " + belial.PesoMedio() + " gramas por pau.");

            // Um pequeno teste de validação
            int pesoIntroduzido = 10;
            bool valido = True;

            if (pesoIntroduzido <= 0)
                pesoIntroduzido = 1;
                valido = False;

            Console.WriteLine("PesoIntroduzido=" + pesoIntroduzido + ", valido=" + valido);

            // Teste de alteração de peso
            PausDeGiz paudegiz = new PausDeGiz(10);
            Console.WriteLine("O pau tem " + paudegiz.Peso + " gramas.");
            fabrica.MudarPeso(paudegiz, 20);
            Console.WriteLine("O pau tem " + paudegiz.Peso + " gramas. Devia ter 20, tem?");

            Console.WriteLine("<<Pode carregar em qualquer tecla para concluir este exemplo.>>");
            Console.ReadKey();
        }
    }
}
