using System;
using System.Collections.Generic;
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
        public void save()              // zapisuje stan radaru do pliku
        {
            // zapisywanie mapy
        }
        public void insert(int x, int y, string s)
        {
            Console.SetCursorPosition(x - 1, y - 1);
            Console.Write(s);
        }
        public string kierunek(int d)
        {
            switch(d)
            {
                case 0: return "[E] Wschod";
                case 1: return "[SE] Poludniowy Wschod";
                case 2: return "[S] Poludnie";
                case 3: return "[SW] Poludniowy Zachod";
                case 4: return "[W] Zachod";
                case 5: return "[NZ] Polnocny Zachod";
                case 6: return "[N] Polnocny";
                case 7: return "[NE] Polnocny Wschod";
            }
            return "Nieznany";
        }
        public void czyscKonsole()
        {
            for (int i = 29; i < 45; i++)
                insert(137, i, "                                                                       ");
            for (int i = 45; i < 49; i++)
                insert(137, i, "                                                             ");
        }
        public void pokaInterfejs()              // wyswietla interfejs graficzny programu
        {
            insert(1, 1, "");
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
                insert(119, 32 + i * 2, "[" + Convert.ToString(i + 1) + "] " + r.s[i].typ + " - " + r.s[i].stanLotu);
            }
            Console.ResetColor();
        }
        public void pokaLot(Radar r, int i)
        {
            string xx = "", yy = "";
            if (i>=0)
            {
                czyscKonsole();
                if (r.s[i].x < 19) xx = "0";
                if (r.s[i].y < 10) yy = "0";
                insert(138, 29, "Dane lotu: " + r.s[i].typ);
                insert(138, 30, "----------------------------------------------------------------------");
                insert(138, 32, "Wspolrzedne geograficzne: (4." + yy + r.s[i].y + "' S, 21." + xx + (r.s[i].x+1)/2 + "' E)");
                insert(138, 33, "Pulap: " + r.s[i].h + " m  n.p.m.");
                insert(138, 34, "Predkosc: " + r.s[i].v + " kmph ("+(r.s[i].v*0.62)+" mph)");
                insert(138, 35, "Kierunek: " + kierunek(r.s[i].d));
                insert(138, 37, "[1] Wyswietl/Ukryj traiektorie");
                insert(138, 38, "[2] Zmien Wspolrzedne geograficzne");
                insert(138, 39, "[0] Opusc okno Zarzadzania Lotem");
            }
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
                pozycja.x += pozycja.v/16;                                 // predkosc opadania (mniej-dluzej)
                if (pozycja.v > 35) pozycja.v = pozycja.v * 9 / 10;        // hamowanie (mniej-szybsze)

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
            else pozycja.v = 0;
        }
    }
    class Trasa
    {
        public int x, y, h, d, v;
        public Trasa(int xx,int yy, int hh, int dd, int vv)
        {
            x = xx;
            y = yy;
            h = hh;
            d = dd;
            v = vv;
        }
    }
    class Statek
    {
        public string typ,stanLotu;
        public int x, y, h, d, v;   // [x,y,h]- wspolrzedne, d- kierunek, v- predkosc
        public int szybkosc;
        public int trajektoria;
        List<Trasa> tr;             // traiektoria
        public Statek(string[] dane)
        {
            typ = dane[0];
            x = int.Parse(dane[1]);
            y = int.Parse(dane[2]);
            h = int.Parse(dane[3]);
            d = int.Parse(dane[4]);
            v = int.Parse(dane[5]);
            szybkosc = 0;
            trajektoria = 0;
            ustalTrajektorie(x, y, d);
            stanLotu = "Spoko";
        }
        public Statek(int xx,int yy,int hh,int dd,int vv)
        {
            typ = "";
            x = xx;
            y = yy;
            h = hh;
            d = dd;
            v = vv;
            szybkosc = 0;
            trajektoria = 0;
            tr = new List<Trasa>();
            stanLotu = "";
        }
        public void ustalTrajektorie(int x_cel, int y_cel, int direct)
        {
            tr = new List<Trasa>();
            tr.Add(new Trasa(x, y, h, d, v));
            double bok = v * v * 0.00000670162031;     // dlugosc boku osmiokata
            if (x_cel != x || y_cel != y)           // znajdz trase do podanego celu
            {
                Statek[] s = new Statek[4];    // tworzy trzon tr s[0], bok okregu s[1], prosta s[2], prostopadla s[3]

                int git = 0;
                double[] dl_bok = new double[8];
                for(int jj=0;jj<8;jj++)             // dlugosci odpowiednich bokow
                {
                    if (d % 4 == 0) dl_bok[jj] = Math.Round(bok * 2);         // poziom
                    else if (d % 4 == 2) dl_bok[jj] = Math.Round(bok * 1);    // pion
                    else if (d % 2 == 1) dl_bok[jj] = Math.Round(bok * 0.71); // skos
                }
                int xd = -1, x1, y1;

                s[0] = new Statek(x,y,h,d,v);
                s[0].d = d;
                s[0].tr.Add(new Trasa(x, y, h, d, v));    // trzon trajektorii s[0]

                int ile = 0;
                while (ile<8 && git<1)
                {
                    int dd = s[0].d;    // ostatni kierunek na trzonie s[0]

                    s[1] = new Statek(x, y, h, d, v);
                    s[1].tr.Add(new Trasa(x, y, h, dd, v));             // bok okregu s[1]

                    for (int ii = 0; ii < dl_bok[d] - 1; ii++)          // dodaje do trajektorii bok
                        plusTrajektorie(ref s[1].tr, s[1].d);

                    x1 = s[1].tr[s[1].tr.Count - 1].x;                  // ostatni x s[1]
                    y1 = s[1].tr[s[1].tr.Count - 1].y;                  // ostatni y s[1]

                    s[2] = new Statek(x1, y1, h, d, v);
                    s[2].tr.Add(new Trasa(x1, y1, h, dd, v));           // tworzy prosta s[2]
                    int i2 = 0,i3 = 0;
                    while (s[2].tr[i2].x < 96 && s[2].tr[i2].y < 32 && s[2].tr[i2].x > 0 && s[2].tr[i2].y > 0)  // tworzy trajektorie s3
                    {
                        plusTrajektorie(ref s[2].tr, s[2].tr[i2].d);     // buduje prosta s[2]
                        i2 = s[2].tr.Count - 1;
                        i3 = 0;
                        s[3] = new Statek(s[2].tr[i2].x, s[2].tr[i2].y, h, d, v);
                        s[3].tr.Add(new Trasa(s[2].x, s[2].y, h, dd, v));     // tworzy prosta s[3]
                        while (s[3].tr[i3].x < 96 && s[3].tr[i3].y < 32 && s[3].tr[i3].x > 0 && s[3].tr[i3].y > 0)
                        {
                            plusTrajektorie(ref s[3].tr, s[3].tr[i3].d);     // buduje prosta s[3]
                            i3 = s[3].tr.Count - 1;
                            if (s[3].tr[i3].x == x_cel && s[3].tr[i3].y == y_cel)
                            {
                                git = 1;
                            }
                            if (git == 1) break;
                        }
                        if (git == 1) break;
                    }
                    if (git == 0) s[0].d = (s[1].d + xd + 8) % 8;
                    s[0].tr.AddRange(s[1].tr);
                }

                if (git == 1) tr.AddRange(s[0].tr);   // dodanie tajektorii s[0] do trajektorii samolotu
            }

            int i = tr.Count - 1;
            while (tr[i].x < 96 && tr[i].y < 32 && tr[i].x > 0 && tr[i].y > 0)  // kontynuluj lot prosto
            {
                plusTrajektorie(ref tr,tr[i].d);    // dodaje nastepna kratke
                i = tr.Count - 1;
            }
        }
        public void pokaTrajektorie()
        {
            Baza b =new Baza();
            for(int i=1;i<tr.Count;i++)
            {
                if(tr[i].x > 0 && tr[i].y>0 && tr[i].x<97 && tr[i].y<33)
                {
                    if (tr[i].d % 4 == 0 && tr[i].x % 2 == 0) b.insert(11 + tr[i].x, 18 + tr[i].y, " ");
                    else b.insert(11 + tr[i].x, 18 + tr[i].y, "^");
                }
            }
        }
        public void aktualizujTrajektorie()
        {
            tr.RemoveAt(0);
        }
        public void zmienTrajektorie()
        {
            Baza b = new Baza();
            b.insert(138, 41, "Podaj nowe wspolrzedne: S 21.");
            int y = Convert.ToInt32(Console.ReadLine());
            b.insert(169, 41, ", E 4.");
            int x = Convert.ToInt32(Console.ReadLine())*2;
            ustalTrajektorie(x, y, d);
            b.insert(138, 40, "                                         ");
        }
        public void plusTrajektorie(ref List<Trasa> tr,int dd)
        {
            int i = tr.Count-1;
            int xx = tr[i].x, yy = tr[i].y, hh = tr[i].h, vv = tr[i].v, d=dd;
            if (tr[i].d != dd) d = (d+7)%8;
            switch (d)
            {
                case 0: xx++; break;
                case 1: xx += 2; yy++; break;
                case 2: yy++; break;
                case 3: xx -= 2; yy++; break;
                case 4: xx--; break;
                case 5: xx -= 2; yy--; break;
                case 6: yy--; break;
                case 7: xx += 2; yy--; break;
            }
            vv++;
            tr.Add(new Trasa(xx, yy, hh, dd, vv));
        }
        public int getDTrajektorii()
        {
            return tr[1].d;
        }
    }
    class Radar
    {
        public List<Statek> s = new List<Statek>();
        string[] mapa = System.IO.File.ReadAllLines("mapa.txt");
        public int run=-1;
        public void pokaRadar()
        {
            Baza b = new Baza();
            for (int i = 0; i < mapa.Length; i++)
                b.insert(10, 18 + i, mapa[i]);
            for (int i = 0; i < s.Count; i++)
                {
                    s[i].d = s[i].getDTrajektorii();
                    s[i].szybkosc += 1;
                    if ((32000 / s[i].v) <= s[i].szybkosc && s[i].d % 2 == 1)        // 560km/h - 1k/2.23s      (skos)
                    {
                        switch (s[i].d)
                        {
                            case 1:s[i].x = s[i].x + 2; s[i].y++; break;
                            case 3:s[i].x = s[i].x - 2; s[i].y++; break;
                            case 5:s[i].x = s[i].x - 2; s[i].y--; break;
                            case 7:s[i].x = s[i].x + 2; s[i].y--; break;
                        }
                        s[i].szybkosc = 0;
                        s[i].aktualizujTrajektorie();
                    }
                    else if ((22400 / s[i].v) <= s[i].szybkosc && s[i].d % 4 == 2)   // 560km/h - 1k/2s         (pion)
                    {
                        switch (s[i].d)
                        {
                            case 2:s[i].y++; break;
                            case 6:s[i].y--; break;
                        }
                        s[i].szybkosc = 0;
                        s[i].aktualizujTrajektorie();
                    }
                    else if ((11200 / s[i].v) <= s[i].szybkosc && s[i].d % 4 == 0)    // 560km/h - 1k/1s         (poziom)
                    {
                        switch (s[i].d)
                        {
                            case 0:s[i].x++; break;
                            case 4:s[i].x--; break;
                        }
                        s[i].szybkosc = 0;
                        s[i].aktualizujTrajektorie();
                    }
                    if (s[i].trajektoria == 1) { s[i].pokaTrajektorie(); }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    b.insert(11 + s[i].x, 18 + s[i].y, s[i].typ);
                    Console.ResetColor();
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
            Trasa losowySamolot = new Trasa(0,0,0,0,0);

            b.pokaInterfejs();
            int ms = 0 ;
            int lot = -1;
            int time = 100;
            int[] x = new int[1];
            x[0] = 0;
            for(; ; )
            {
                if (radar.run < 0)  b.pokaInterfejs();
                else radar.pokaRadar();                             // aktualizuje i wyswietla radar
                b.pokaListeLotow(radar,x[0]);                       // wypisuje liste lotow do podgledu/edycji
                b.insert(200,26,Convert.ToString(ms/10+" s"));      // wypisuje czas trwania programu
                b.insert(212, 51, Convert.ToString("."));           // wypisuje nic na koncu okna
                b.pokaLot(radar, lot);
                
                if (Console.KeyAvailable)                           // pobiera wybrany przycisk
                {
                    char wybor = Console.ReadKey().KeyChar;
                    if(lot>=0)
                    {
                        switch(wybor)
                        {
                            case '0':b.czyscKonsole(); lot = -1; break;
                            case '1': if (radar.s[lot].trajektoria < 1) radar.s[lot].trajektoria++; else radar.s[lot].trajektoria--; break;
                            case '2': radar.s[lot].zmienTrajektorie(); break;
                        }
                    }                                          // zarzadzanie lotem
                    else if (wybor-49 >= 0 && wybor-49 < 10 && wybor-49 <= radar.s.Count)
                    {
                        lot = wybor - 49;
                    }   // wypisuje info o danym locie
                    else if (wybor == 'q')
                    {
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }   // zabija aplikacje
                    else if (wybor == 'e')
                    {

                    }   // info o wlascicielach
                    else if (wybor == 'w')
                    {
                        losowySamolot = new Trasa(1,2,0,0,200);
                    }   // pusc samolot
                    else if (wybor == 'r')
                    {
                        radar.run *= -1;
                    }   // wlaczy/wylacz radar
                    else if (wybor == ']')
                    {
                        if(time>15)time -= 15;
                    }   // przyspiesz czas
                    else if (wybor == '[')
                    {
                        time += 15;
                    }   // przyspiesz czas
                }
                Thread.Sleep(time);
                ms++;
                if (losowySamolot.v != 0) b.pokaSamolot(ref losowySamolot); // ladujacy samolot
            }
        }
    }
} 