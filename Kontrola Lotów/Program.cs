using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading;

namespace Kontrola_Lotów
{
    class Baza
    {
        string[] wierza;
        public Baza(Radar r)            // tworzenie mapy wymaga wczytania 2 plikow: wierza.txt, radar.txt
        {
            wierza = System.IO.File.ReadAllLines("wierza.txt");
            string[] ObiektNaRadarze = System.IO.File.ReadAllLines("radar.txt");  // zawartosc pliku radar w liniach
            int rows = int.Parse(ObiektNaRadarze[0]);
            for (int i = 1; i < ObiektNaRadarze.Length; i++)
            {
                r.s.Add(new Statek(ObiektNaRadarze[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)));
            }
        }
        public Baza() { }
        public void insert(int x, int y, string s)
        {
            Console.SetCursorPosition(x - 1, y - 1);
            Console.Write(s);
        }
        public void czyscKonsole()
        {
            for(int i=29;i<49;i++)
            {
                insert(136, i, "                                                                       ");
            }
        }
        public void save()              // zapisuje stan radaru do pliku
        {
            // zapisywanie mapy
        }
        public void pokaInterfejs()              // wyswietla interfejs graficzny programu
        {
            for (int i = 0; i < wierza.Length; i++)
            {
                Console.WriteLine(wierza[i]);
            }
        }
        public void pokaListeLotow(Radar r, int czy)
        {
            if (czy > 0)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < r.s.Count; i++)
            {
                insert(117, 32 + i * 2, "[" + Convert.ToString(i + 1) + "] " + r.s[i].typ + " - " + r.s[i].stanLotu);
            }
            Console.ResetColor();
        }
        public void pokaLot(Radar r, int i)
        {
            insert(136, 29, "Dane lotu " + r.s[i].typ);
        }
        public void pokaSamolot(ref Trasa pozycja)
        {
            int tmp = 3;
            string[] samolot = System.IO.File.ReadAllLines("samolot.txt");
            for (int i=1;i<22;i++) insert(1, i, wierza[i-1]);
            if (pozycja.x < 116)    // przesuwanie sie w prawo
            {
                if (pozycja.x == 1) tmp = 0;
                
                insert(pozycja.x, pozycja.y + 1, samolot[0 + tmp]);
                insert(pozycja.x + 1, pozycja.y + 2, samolot[1 + tmp]);
                insert(pozycja.x + 7, pozycja.y + 3, samolot[2 + tmp]);

                if (pozycja.y < 4 ) pozycja.y++;
                pozycja.x += pozycja.s/16;                                  // predkosc opadania (mniej-dluzej)
                if (pozycja.s > 35) pozycja.s = pozycja.s * 9 / 10;        // hamowanie (mniej-szybsze)

                insert(6, 3, "|    ||");
                insert(6, 4, "|    ||");
                insert(6, 5, "|    ||");
            }
            else if (pozycja.y<22)   // przesuwanie w dol
            {
                pozycja.y++;
                pozycja.x++;

                if (pozycja.y < 7)
                {
                    pozycja.y--;
                    pozycja.x += 4;
                    insert(pozycja.x + 1, pozycja.y + 0, samolot[6]);
                    insert(pozycja.x + 0, pozycja.y + 1, samolot[7]);
                    insert(pozycja.x + 3, pozycja.y + 2, samolot[8]);
                    insert(pozycja.x + 0, pozycja.y + 3, samolot[9]);
                    insert(pozycja.x + 0, pozycja.y + 4, samolot[10]);
                    insert(pozycja.x + 6, pozycja.y + 5, samolot[11]);
                    pozycja.y++;
                }
                else
                {
                    insert(pozycja.x + 4, pozycja.y + 0, samolot[12]);
                    insert(pozycja.x + 6, pozycja.y + 1, samolot[13]);
                    insert(pozycja.x + 0, pozycja.y + 2, samolot[14]);
                    insert(pozycja.x + 1, pozycja.y + 3, samolot[15]);
                    insert(pozycja.x + 8, pozycja.y + 4, samolot[16]);
                    insert(pozycja.x + 9, pozycja.y + 5, samolot[17]);
                }
                for (int i = 22; i < 28; i++) insert(1, i, wierza[i - 1]);
            }
            else pozycja.s = 0;
        }
    }
    class Trasa
    {
        public int x, y, s;
        public Trasa()
        {
            x = 1;
            y = 2;
            s = 200;
        }
    }
    class Statek
    {
        public string typ,stanLotu;
        public int x, y, h, d, v;   // [x,y,h]- wspolrzedne, d- kierunek, v- predkosc
        public int szybkosc;
        List<Trasa> trajektoria;
        public Statek(string[] tmp)
        {
            typ = tmp[0];
            x = int.Parse(tmp[1]);
            y = int.Parse(tmp[2]);
            h = int.Parse(tmp[3]);
            d = int.Parse(tmp[4]);
            v = int.Parse(tmp[5]);
            szybkosc = 10000;
            stanLotu = "Spoko";
        }
    }
    class Radar
    {
        public List<Statek> s = new List<Statek>();
        public void pokaRadar()
        {
            Baza b = new Baza();
            for(int i=0;i<s.Count;i++)
            {
                s[i].szybkosc++;
                if((1000/s[i].v)<=s[i].szybkosc)
                {
                    s[i].szybkosc = 0;
                    switch (s[i].d)
                    {
                        case 0: b.insert(14 + s[i].x, 19 + s[i].y, "   "); s[i].x++; break;
                        case 1: b.insert(14 + s[i].x, 19 + s[i].y, "   "); s[i].x++; s[i].y++; break;
                        case 2: b.insert(14 + s[i].x, 19 + s[i].y, "   "); s[i].y++; break;
                        case 3: b.insert(14 + s[i].x, 19 + s[i].y, "   "); s[i].x--; s[i].y++; break;
                        case 4: b.insert(14 + s[i].x, 19 + s[i].y, "   "); s[i].y--; break;
                        case 5: b.insert(14 + s[i].x, 19 + s[i].y, "   "); s[i].x--; s[i].y--; break;
                        case 6: b.insert(14 + s[i].x, 19 + s[i].y, "   "); s[i].x--; break;
                        case 7: b.insert(14 + s[i].x, 19 + s[i].y, "   "); s[i].x++; s[i].y--; break;
                    }
                    b.insert(14+s[i].x, 19+s[i].y, s[i].typ);
                }
            }

        }
        public void Kolizja()
        {
            for (int i = 0; i < s.Count; i++)
            {
                for (int j = i + 1; j < s.Count; j++)
                {
                    if (s[i].h == s[j].h)     // sprawdzenie wysokosci
                    {
                        ;
                    }
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(213, 52);
            Console.SetBufferSize(213, 52);
            Console.Title = "Kontrola Lotow";

            Radar radar = new Radar();
            Baza b = new Baza(radar);
            Trasa losowySamolot = new Trasa();

            b.pokaInterfejs();
            int ms = 0 ;
            int czy = 0;
            for(; ; )
            {
                radar.pokaRadar();                                  // aktualizuje i wyswietla radar
                b.pokaListeLotow(radar,czy);                        // wypisuje liste lotow do podgledu/edycji
                b.insert(200,26,Convert.ToString(ms/10+" s"));      // wypisuje czas trwania programu
                b.insert(212, 51, Convert.ToString("."));           // wypisuje nic na koncu okna
                
                if (Console.KeyAvailable)                           // pobiera wybrany przycisk
                {
                    char wybor = Console.ReadKey().KeyChar;
                    int i = wybor - 49;
                    if(czy>0)
                    {
                        switch(wybor)
                        {
                            case '0':czy = 0; b.czyscKonsole(); break;
                        }
                    }
                    else if (i >= 0 && i < 10 && i <= radar.s.Count)
                    {
                        b.pokaLot(radar, i);            // wypisuje info o danym locie
                        czy++;
                    }
                    else if (wybor == 'q')
                    System.Diagnostics.Process.GetCurrentProcess().Kill();      // zabija aplikacje
                    else if (wybor == 'e')
                    {
                        // wypisz info o autorach
                    }
                    else if (wybor == 'w')
                    {
                        losowySamolot = new Trasa();
                    }
                }
                
                Thread.Sleep(100);
                ms++;

                if (losowySamolot.s != 0) b.pokaSamolot(ref losowySamolot);
            }
        }
    }
} 