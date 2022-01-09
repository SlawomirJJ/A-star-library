using System;
using System.Collections.Generic;
using System.Linq;
using static AstarLibrary.Algorithm;

namespace AstarLibrary
{
    public static class ForUser
    {
        static List<PoleModel> Pola = new List<PoleModel>();
        static List<PoleModel> Sciezka = new List<PoleModel>();
        static int DlugoscSiatki;

        // Ustaw pole startowe
        public static void PoleStart(int StartId)
        {
            if (Pola.Count!=0)
            {
                for (int i = 0; i < Pola.Count; i++)
                {
                    if (Pola[i].StartKon == 's')
                    {
                        Pola[i].StartKon = 'n';
                        break;
                    }
                }
                Pola.ElementAt(StartId - 1).StartKon = 's';
            }
        }

        // Ustaw pole końcowe
        public static void PoleKoniec(int KoniecId)
        {
            if (Pola.Count != 0)
            {
                for (int i = 0; i < Pola.Count; i++)
                {
                    if (Pola[i].StartKon == 'k')
                    {
                        Pola[i].StartKon = 'n';
                        break;
                    }
                }
                Pola.ElementAt(KoniecId - 1).StartKon = 'k';
            }          
        }

        // Ustaw ilość pól na jednym boku
        public static void Rozmiar(int Rozmiar)
        {
            Pola.Clear();
            for (int i = 1; i <= Rozmiar * Rozmiar; i++)
            {
                Pola.Add(new PoleModel(i, 0, 0, Rozmiar) { StartKon = 'n', Osiagalny = true });
            }
            DlugoscSiatki = Rozmiar;
        }

        // Znajdź drogę do celu
        public static List<int> WyznaczenieTrasy(int IdEnd)
        {
            PoleKoniec(IdEnd);
            Sciezka.Clear();
            Sciezka = Algorytm(Pola);
            return Komunikaty(Sciezka, DlugoscSiatki);
        }

        public static List<int> WyznaczenieTrasy(int IdStart,int IdEnd)
        {
            PoleStart( IdStart);
            PoleKoniec(IdEnd);
            Sciezka.Clear();
            Sciezka = Algorytm(Pola);
            return Komunikaty(Sciezka, DlugoscSiatki);
        }

        public static List<int> WyznaczenieTrasy()
        {
            Sciezka.Clear();
            Sciezka = Algorytm(Pola);
            return Komunikaty(Sciezka, DlugoscSiatki);
        }

        // Komunikaty informujące co powinien zrobić robot
        static List <int> Komunikaty(List< PoleModel> Sciezka, int DlugoscSiatki)
        {
            List<int> komunikaty = new List<int>();
            for (int i = 0; i < Sciezka.Count; i++)
            {
                if (Sciezka[i].Rodzic.Id== Sciezka[i].Id-1)
                {
                    komunikaty.Add(1); // w prawo
                }
                else if (Sciezka[i].Rodzic.Id == Sciezka[i].Id + 1)
                {
                    komunikaty.Add(2); // w lewo
                }
                else if (Sciezka[i].Rodzic.Id == Sciezka[i].Id + DlugoscSiatki)
                {
                    komunikaty.Add(3);  // do tyłu
                }
                else if (Sciezka[i].Rodzic.Id == Sciezka[i].Id - DlugoscSiatki)
                {
                    komunikaty.Add(4); // do przodu
                }
                else if (Sciezka[i].Rodzic.Id == Sciezka[i].Id + DlugoscSiatki + 1)
                {
                    komunikaty.Add(5); // do tyłu w lewo (po ukosie)
                }
                else if (Sciezka[i].Rodzic.Id == Sciezka[i].Id + DlugoscSiatki - 1)
                {
                    komunikaty.Add(6); ; // do tyłu w prawo (po ukosie)
                }
                else if (Sciezka[i].Rodzic.Id == Sciezka[i].Id - DlugoscSiatki + 1)
                {
                    komunikaty.Add(7); // do przodu w lewo (po ukosie)
                }
                else if (Sciezka[i].Rodzic.Id == Sciezka[i].Id - DlugoscSiatki - 1)
                {
                    komunikaty.Add(8); // do przodu w prawo (po ukosie)
                }
            }
            return komunikaty;
        }

        // Ustawienie pól niedostępnych
        public static void DodaniePrzeszkod(int Nrprzeszkody)
        {
            PoleModel start = null;
            PoleModel koniec = null;
            int sprawdzenie = 0;
            for (int j = 0; j < Pola.Count; j++)
            {
                if (Pola.ElementAt(j).StartKon == 's')
                {
                    start = Pola.ElementAt(j);
                }
                else if (Pola.ElementAt(j).StartKon == 'k')
                {
                    koniec = Pola.ElementAt(j);
                }
            }
            if (start != null)
            {
                if (Nrprzeszkody != start.Id - 1)
                {
                    sprawdzenie++;
                }
            }
            if (koniec != null)
            {
                if (Nrprzeszkody != start.Id - 1)
                {
                    sprawdzenie++;
                }
            }
            if (sprawdzenie == 2)
            {
                Pola[Nrprzeszkody].Osiagalny = false;
            }
        }


    }
}
