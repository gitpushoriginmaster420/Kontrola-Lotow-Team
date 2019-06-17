using System;
using System.Collections.Generic;
using System.Threading;

namespace Kontrola_Lotów
{
    class Baza
    {
        string[] fax;
        string[] wierza;
        public Baza(Radar r)            // tworzenie radaru wymaga wczytania 3 plikow: wierza.txt, radar.txt, budynki.txt
        {
            wierza = System.IO.File.ReadAllLines("wierza.txt");
            fax = System.IO.File.ReadAllLines("fax.txt");

            string[] ObiektNaRadarze = System.IO.File.ReadAllLines("radar.txt");  // zawartosc pliku radar w liniach
            for (int i = 0; i < ObiektNaRadarze.Length; i++)
                r.s.Add(new Statek(ObiektNaRadarze[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)));

            string[] ObiektNaMapie = System.IO.File.ReadAllLines("budynki.txt");  // zawartosc pliku budynki w liniach
            for (int i = 0; i < ObiektNaMapie.Length; i++)
                r.b.Add(new Budynek(ObiektNaMapie[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)));
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
            switch (d)
            {
                case 0: return "[E] Wschod";
                case 1: return "[SE] Poludniowy Wschod";
                case 2: return "[S] Poludnie";
                case 3: return "[SW] Poludniowy Zachod";
                case 4: return "[W] Zachod";
                case 5: return "[NW] Polnocny Zachod";
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
            if (czy > -1)
                Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int i = 0; i < r.s.Count; i++)
            {
                insert(119, 32 + i * 2, "[" + Convert.ToString(i + 1) + "] " + r.s[i].typ + " - " + r.s[i].stanLotu);
            }
            insert(119, 32 + r.s.Count * 2, "                 ");
            Console.ResetColor();
        }
        public void pokaListeBudynkow(Radar r)
        {
            Baza b = new Baza();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            b.pokaKonsole(r.skala, 0);
            if(r.run>0)b.pokaListeLotow(r, 1);
            Console.ResetColor();

            for (int i = 0; i < r.b.Count/2; i++)
                insert(138, 40 + i, Convert.ToString(r.b[i].h)+"m - "+r.b[i].nazwa);

            for (int i = r.b.Count/2; i < r.b.Count; i++)
                insert(170, 34 + i, Convert.ToString(r.b[i].h) + "m - " + r.b[i].nazwa);

            char wyb = Console.ReadKey().KeyChar;
            b.czyscKonsole();
        }
        public void pokaLot(Radar r, int i)
        {
            string xx = "", yy = "";
            if (i >= 0)
            {
                czyscKonsole();
                if (r.s[i].x < 19) xx = "0";
                if (r.s[i].y < 10) yy = "0";
                insert(138, 29, "Dane lotu: " + r.s[i].typ);
                insert(138, 30, "----------------------------------------------------------------------");
                if (r.s[i].x < 0 && r.s[i].y >= 0)
                    insert(138, 32, "Wspolrzedne geograficzne: (4°" + yy + r.s[i].y + "' S, 20°" + (60 - (Math.Abs(r.s[i].x) + 1) / 2) + "' E)");
                else if (r.s[i].x >= 0 && r.s[i].y < 0)
                    insert(138, 32, "Wspolrzedne geograficzne: (3°" + (60 + r.s[i].y) + "' S, 21°" + xx + (r.s[i].x + 1) / 2 + "' E)");
                else if (r.s[i].x < 0 && r.s[i].y < 0)
                    insert(138, 32, "Wspolrzedne geograficzne: (3°" + (60 + r.s[i].y) + "' S, 20°" + (60 - (Math.Abs(r.s[i].x + 1)) / 2) + "' E)");
                else
                    insert(138, 32, "Wspolrzedne geograficzne: (4°" + yy + r.s[i].y + "' S, 21°" + xx + (r.s[i].x + 1) / 2 + "' E)");
                insert(138, 33, "Pulap: " + r.s[i].h + " m  n.p.m.");
                insert(138, 34, "Predkosc: " + r.s[i].v + " kmph (" + (r.s[i].v * 0.62) + " mph)");
                insert(138, 35, "Kierunek: " + kierunek(r.s[i].d));
                insert(138, 37, "[1] Wyswietl/Ukryj traiektorie");
                insert(138, 38, "[2] Zmien Wspolrzedne geograficzne");
                insert(138, 39, "[0] Opusc okno Zarzadzania Lotem");
            }
        }
        public void pokaKonsole(int skala,int gen)
        {
            string onoff = "OFF";
            if (gen > 0) onoff = "ON ";
            insert(138, 29, "* Zarzadzanie Systemem *");
            insert(138, 30, "----------------------------------------------------------------------");
            insert(138, 32, "[M] Wczytaj inna mape                      [<] Spowolnij czas");
            insert(138, 34, "[G] Generowanie lotow: "+onoff);insert(181,34,"[>] Przyspiesz czas");
            insert(138, 36, "[W] Pusc animacje ladujacego samolotu      [/] Ustaw domyslny czas");
            insert(138, 38, "[X] Niespodziewana sytuacja");
            if (skala == 20) insert(181,38,"[B] Wyswietl liste budynkow");
        }
        public void pokaSamolot(ref Trasa pozycja)
        {
            int tmp = 3;
            string[] samolot = System.IO.File.ReadAllLines("samolot.txt");
            for (int i = 1; i < 22; i++) insert(1, i, wierza[i - 1]);
            if (pozycja.x < 116)    // przesuwanie sie w prawo
            {
                if (pozycja.x == 1) tmp = 0;

                insert(pozycja.x, pozycja.y + 1, samolot[0 + tmp]);
                insert(pozycja.x + 1, pozycja.y + 2, samolot[1 + tmp]);
                insert(pozycja.x + 7, pozycja.y + 3, samolot[2 + tmp]);

                if (pozycja.y < 4) pozycja.y++;
                pozycja.x += pozycja.v / 16;                                 // predkosc opadania (mniej-dluzej)
                if (pozycja.v > 35) pozycja.v = pozycja.v * 9 / 10;        // hamowanie (mniej-szybsze)

                insert(6, 3, "|    ||");
                insert(6, 4, "|    ||");
                insert(6, 5, "|    ||");
            }
            else if (pozycja.y < 22)   // przesuwanie w dol
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
        public void pokaFax()
        {
            insert(70, 8, ".-----------------------------------------------------------------------------.");
            for (int i=0;i<fax.Length;i++)
            {
                insert(70, 9 + i, "|                                                                             |");
                insert(73, 9 + i, fax[i]);
            }
            insert(70, 21, "'-----------------------------------------------------------------------------'");
            char w = Console.ReadKey().KeyChar;
        }
    }
    class Trasa
    {
        public int x, y, h, d, v;
        public Trasa(int xx, int yy, int hh, int dd, int vv)
        {
            x = xx;
            y = yy;
            h = hh;
            d = dd;
            v = vv;
        }
    }
    class Budynek
    {
        public int x, y, h;
        public string nazwa;
        public Budynek(string[] dane)
        {
            nazwa = dane[0];
            y = int.Parse(dane[1]);
            x = int.Parse(dane[2]);
            h = int.Parse(dane[3]);
        }

    }
    class Statek
    {
        public string typ, stanLotu;
        public int x, y, h, d, v;   // [x,y,h]- wspolrzedne, d- kierunek, v- predkosc
        public int szybkosc;
        public int trajektoria;
        List<Trasa> tr;             // traiektoria
        public Statek(string[] dane)
        {
            typ = dane[0];
            x = int.Parse(dane[1]) * 2;
            y = int.Parse(dane[2]);
            h = int.Parse(dane[3]);
            d = int.Parse(dane[4]);
            v = int.Parse(dane[5]);
            szybkosc = 0;
            trajektoria = 0;
            ustalTrajektorie(x, y, d, 1);
            stanLotu = "Spoko";
        }
        public Statek(string typtyp, int xx, int yy, int hh, int dd, int vv)
        {
            typ = typtyp;
            x = xx;
            y = yy;
            h = hh;
            d = dd;
            v = vv;
            szybkosc = 0;
            trajektoria = 0;
            ustalTrajektorie(x, y, d, 1);
            stanLotu = "Spoko";
        }
        public Statek(int xx, int yy, int hh, int dd, int vv)
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
        public void ustalTrajektorie(int x_cel, int y_cel, int direct,int skala)
        {
            tr = new List<Trasa>();
            //plusTrajektorie(ref tr, d);
            double bok = v * v * 0.00000670162031 * skala;     // dlugosc boku osmiokata
            if (x_cel != x || y_cel != y)           // znajdz trase do podanego celu
            {
                Statek[] s = new Statek[5];    // tworzy trzon tr s[0], bok okregu s[1], prosta s[2], prostopadla s[3]
                Baza b = new Baza();
                int git = 0;
                double[] dl_bok = new double[8];
                for (int jj = 0; jj < 8; jj++)             // dlugosci odpowiednich bokow
                {
                    if (jj % 4 == 0) dl_bok[jj] = Math.Round(bok * 2);         // poziom
                    else if (jj % 4 == 2) dl_bok[jj] = Math.Round(bok * 1);    // pion
                    else if (jj % 2 == 1) dl_bok[jj] = Math.Round(bok * 0.71); // skos
                }
                int xd = -1, x1, y1, x0, y0;

                s[0] = new Statek(x, y, h, d, v);
                s[0].d = d;
                s[0].tr.Add(new Trasa(x, y, h, d, v));    // trzon trajektorii s[0]

                s[4] = new Statek(x, y, h, (d + 4) % 8, v);       // prosta linia - trasa za samolotem s[4]
                s[4].d = (d + 4) % 8;
                s[4].tr.Add(new Trasa(x, y, h, (d + 4) % 8, v));

                int i4 = 0;
                while (s[4].tr[i4].x < 120 && s[4].tr[i4].y < 44 && s[4].tr[i4].x > -24 && s[4].tr[i4].y > -12)  // wyznacz linie za samolotem
                {
                    plusTrajektorie(ref s[4].tr, s[4].tr[i4].d);    // dodaje nastepna kratke
                    i4 = s[4].tr.Count - 1;
                }

                int ile = 0;
                while (ile < 8 && git < 1)   // szuka mozliwosci poki co w lewo
                {
                    int dd = s[0].d;    // ostatni kierunek na trzonie s[0]
                    x0 = s[0].tr[s[0].tr.Count - 1].x;
                    y0 = s[0].tr[s[0].tr.Count - 1].y;

                    s[1] = new Statek(x0, y0, h, dd, v);
                    s[1].tr.Add(new Trasa(x0, y0, h, dd, v));           // bok okregu s[1]

                    for (int ii = 0; ii < dl_bok[dd] - 1; ii++)         // dodaje do trajektorii bok
                        plusTrajektorie(ref s[1].tr, s[1].d);

                    x1 = s[1].tr[s[1].tr.Count - 1].x;                  // ostatni x s[1]
                    y1 = s[1].tr[s[1].tr.Count - 1].y;                  // ostatni y s[1]

                    s[2] = new Statek(x1, y1, h, dd, v);
                    s[2].tr.Add(new Trasa(x1, y1, h, dd, v));           // tworzy prosta s[2]
                    int i2 = 0, i3 = 0;
                    while (s[2].tr[i2].x < 96 && s[2].tr[i2].y < 32 && s[2].tr[i2].x > 0 && s[2].tr[i2].y > 0)  // tworzy trajektorie s[2]
                    {
                        plusTrajektorie(ref s[2].tr, s[2].tr[i2].d);    // buduje prosta s[2]
                        i2 = s[2].tr.Count - 1;
                        i3 = 0;
                        s[3] = new Statek(s[2].tr[i2].x, s[2].tr[i2].y, h, d, v);
                        s[3].tr.Add(new Trasa(s[2].tr[i2].x, s[2].tr[i2].y, h, (8 + dd + xd) % 8, v));              // tworzy prosta s[3]
                        while (s[3].tr[i3].x < 96 && s[3].tr[i3].y < 32 && s[3].tr[i3].x > 0 && s[3].tr[i3].y > 0)  //tworzy tr s[3]
                        {
                            plusTrajektorie(ref s[3].tr, s[3].tr[i3].d);     // buduje prosta s[3]
                            i3 = s[3].tr.Count - 1;
                            if (s[3].tr[i3].x == x_cel && s[3].tr[i3].y == y_cel)
                            {
                                git = 1;
                                s[2].tr.RemoveAt(s[2].tr.Count - 1);
                                s[2].tr.AddRange(s[3].tr);
                                s[1].tr.AddRange(s[2].tr);
                            }
                            if (git == 1) break;
                        }
                        if (git == 1) break;
                    }
                    if (git == 0) s[0].d = (s[1].d + xd + 8) % 8;
                    s[1].tr.RemoveAt(0);
                    s[0].tr.AddRange(s[1].tr);
                    ile++;
                }

                if (git == 1) tr.AddRange(s[0].tr);   // dodanie tajektorii s[0] do trajektorii samolotu
                else tr.Add(new Trasa(x, y, h, d, v));
            }
            else tr.Add(new Trasa(x, y, h, d, v));
            int i = tr.Count - 1;
            while (tr[i].x < 120 && tr[i].y < 44 && tr[i].x > -24 && tr[i].y > -12)  // kontynuluj lot prosto
            {
                plusTrajektorie(ref tr, tr[i].d);    // dodaje nastepna kratke
                i = tr.Count - 1;
            }
        }
        public void pokaTrajektorie()
        {
            Baza b = new Baza();
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 1; i < tr.Count; i++)
            {
                if (tr[i].x > 0 && tr[i].y > 0 && tr[i].x < 97 && tr[i].y < 33)
                {
                    if (tr[i].d % 4 == 0 && tr[i].x % 2 == 0) b.insert(11 + tr[i].x, 18 + tr[i].y, " ");
                    else b.insert(11 + tr[i].x, 18 + tr[i].y, "^");
                    //else b.insert(11 + tr[i].x, 18 + tr[i].y, Convert.ToString(tr[i].d));
                }
            }
            Console.ResetColor();
        }
        public void aktualizujTrajektorie()
        {
            tr.RemoveAt(0);
        }
        public void zmienTrajektorie(int skala)
        {
            Baza b = new Baza();
            b.insert(138, 41, "Podaj nowe wspolrzedne: E 21°");
            int x = Convert.ToInt32(Console.ReadLine()) * 2;
            b.insert(169, 41, ", S 4°");
            int y = Convert.ToInt32(Console.ReadLine());
            ustalTrajektorie(x, y, d, skala);
            b.insert(138, 40, "                                         ");
        }
        public void plusTrajektorie(ref List<Trasa> tr, int dd)
        {
            int i = tr.Count - 1;
            int xx = tr[i].x, yy = tr[i].y, hh = tr[i].h, vv = tr[i].v, d = dd;
            if (tr[i].d != dd) d = tr[i].d;
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
            if (tr.Count > 1)
                return tr[1].d;
            else
                return -1;
        }
    }
    class Radar
    {
        public List<Statek> s;
        public List<Budynek> b;
        string[] mapa;
        public int run, skala;
        public Radar()
        {
            s = new List<Statek>();
            b = new List<Budynek>();
            mapa = System.IO.File.ReadAllLines("mapa1.txt");
            run = -1;
            skala = int.Parse(mapa[mapa.Length - 1]);
        }
        public void pokaRadar(ref int lot)
        {
            Baza baz = new Baza();
            for (int j = 0; j < mapa.Length - 1; j++) baz.insert(10, 18 + j, mapa[j]);
            for (int i = 0; i < s.Count; i++)
            {
                s[i].d = s[i].getDTrajektorii();
                if (s[i].d < 0)
                {
                    s.RemoveAt(s.IndexOf(s[i]));
                    lot = -1;
                    baz.czyscKonsole();
                }
                else
                {
                    s[i].szybkosc += 1;
                    if ((32000 / s[i].v) / skala <= s[i].szybkosc && s[i].d % 2 == 1)        // 560km/h - 1k/2.23s      (skos)
                    {
                        switch (s[i].d)
                        {
                            case 1: s[i].x = s[i].x + 2; s[i].y++; break;
                            case 3: s[i].x = s[i].x - 2; s[i].y++; break;
                            case 5: s[i].x = s[i].x - 2; s[i].y--; break;
                            case 7: s[i].x = s[i].x + 2; s[i].y--; break;
                        }
                        s[i].szybkosc = 0;
                        s[i].aktualizujTrajektorie();
                    }
                    else if ((22400 / s[i].v) / skala <= s[i].szybkosc && s[i].d % 4 == 2)   // 560km/h - 1k/2s         (pion)
                    {
                        switch (s[i].d)
                        {
                            case 2: s[i].y++; break;
                            case 6: s[i].y--; break;
                        }
                        s[i].szybkosc = 0;
                        s[i].aktualizujTrajektorie();
                    }
                    else if ((11200 / s[i].v) / skala <= s[i].szybkosc && s[i].d % 4 == 0)    // 560km/h - 1k/1s         (poziom)
                    {
                        switch (s[i].d)
                        {
                            case 0: s[i].x++; break;
                            case 4: s[i].x--; break;
                        }
                        s[i].szybkosc = 0;
                        s[i].aktualizujTrajektorie();
                    }
                    if (s[i].trajektoria == 1) { s[i].pokaTrajektorie(); }

                    if (s[i].szybkosc > 0) ;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (s[i].x > 0 && s[i].x <= 96 && s[i].y > 0 && s[i].y <= 32) baz.insert(11 + s[i].x, 18 + s[i].y, s[i].typ);
                    Console.ResetColor();
                }
            }
        }
        public void pokaSkale()
        {
            Baza baz = new Baza();
            for (int i = 1; i < 25; i++)
            {
                baz.insert(9 + i * 4, 17, Convert.ToString(100 + i * 2));
                baz.insert(9 + i * 4, 17, ":");
            }
            for (int i = 0; i < mapa.Length - 1; i++)
            {
                if (i % 2 == 0 && i != 0)
                {
                    baz.insert(6, 18 + i, Convert.ToString(100 + i));
                    baz.insert(6, 18 + i, ":");
                }
            }
        }
        public void zmienMape(int czy)
        {
            Baza baz = new Baza();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            baz.pokaKonsole(skala,0);
            if(czy>0) baz.pokaListeLotow(this,1);
            Console.ResetColor();
            baz.insert(139, 42, "Wybierz mape, ktora chcesz wczytac:");
            baz.insert(141, 44, "[1] Mapa SanAndreas (48 x 32 km)");
            baz.insert(141, 46, "[2] Mapa NewYourk (2,4 x 1,6 km)");
            switch (Console.ReadKey().KeyChar)
            {
                case '1': mapa = System.IO.File.ReadAllLines("mapa1.txt"); skala = int.Parse(mapa[mapa.Length - 1]); break;
                case '2': mapa = System.IO.File.ReadAllLines("mapa2.txt"); skala = int.Parse(mapa[mapa.Length - 1]); break;
            }
            baz.czyscKonsole();
        }
        public void naniesBudynki()
        {
            Baza baz = new Baza();
            Console.ForegroundColor = ConsoleColor.Red;
            for(int i=0;i<b.Count;i++)
            {
                baz.insert(11+b[i].x, 18+b[i].y, "[]");
            }
            Console.ResetColor();
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
        public int Losowanko()
        {
            Baza b = new Baza();
            if (s.Count == 9) return 0; 

            Random rand = new Random();
            int r, eloy, bencx, w = 0, k = 0, p = 0, czy;
            string typ = "";
            eloy = rand.Next(-12, 44);
            //bencx = rand.Next(-24, 120);
            bencx = rand.Next(1, 95);
            czy = rand.Next(0, 30);

            if ((eloy > 32 || eloy < 0 || bencx > 96 || bencx < 0) && czy == 13)
            {
                int los;
                switch(skala)
                {
                    case 1:     // y = -12 44 x = -24 120  x 0 96 y 0 32 , 770 830
                    {
                        los = rand.Next(1,3);
                            if (los == 1) { p = rand.Next(770, 830); w = rand.Next(8000, 12000); typ = "S"; }
                            if (los == 2) { p = rand.Next(250, 350); w = rand.Next(1000, 2000); typ = "Z"; }
                            if (los == 3) { p = rand.Next(150, 300); w = rand.Next(500, 5000); typ = "H"; }
                    }
                    break;
                    case 20:     // 150 300
                    {
                        los = rand.Next(1, 3);
                        if (los == 1) { p = rand.Next(20, 40); w = rand.Next(100, 800); typ = "B"; }
                        if (los == 2) { p = rand.Next(90, 120); w = rand.Next(1000, 2000); typ = "Z"; }
                        if (los == 3) { p = rand.Next(100, 200); w = rand.Next(500, 5000); typ = "H"; }
                    }
                    break;
                }
                los = rand.Next(100, 999);
                typ = typ + Convert.ToString(los);


                if (eloy > 32)     // statek zrespil sie na dolnej polowie mapy
                {
                    if (bencx < 0) k = 7;
                    else if (bencx >= 0 && bencx < 18) k = rand.Next(6, 7);
                    else if (bencx >= 18 && bencx <= 78) k = rand.Next(5, 7);
                    else if (bencx > 78 && bencx <= 96) k = rand.Next(5, 6);
                    else if (bencx > 96) k = 5;
                }
                else if (eloy < 0)     // statek zrespil sie na gornej polowie mapy
                {
                    if (bencx < 0) k = 1;
                    else if (bencx >= 0 && bencx < 18) k =rand.Next(1, 2);
                    else if (bencx >= 18 && bencx <= 78) k = rand.Next(1, 3);
                    else if (bencx > 78 && bencx <= 96) k = rand.Next(2, 3);
                    else if (bencx > 96) k = 3;
                }
                else if (bencx < 0)     // statek zrespil sie po lewej stronie mapy
                {
                    if (eloy >= 0 && eloy < 8) k = rand.Next(6, 7);
                    else if (eloy >= 8 && eloy <= 24) k = rand.Next(5, 7);
                    else if (eloy > 24 && eloy <= 32) k = rand.Next(5, 6);
                }
                else if (bencx > 32)     // statek zrespil sie po prawej stronie mapy
                {
                    if (eloy >= 0 && eloy < 8) k =  rand.Next(6, 7);
                    else if (eloy >= 8 && eloy <= 24) k = rand.Next(5, 7);
                    else if (eloy > 24 && eloy <= 32) k = rand.Next(5, 6);
                }
                s.Add(new Statek(typ, bencx, eloy, w, k, p));
            }
            return 0;
        }
        public void NiebezpieczneZblizenia()
        {

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
            Trasa losowySamolot = new Trasa(0, 0, 0, 0, 0);

            b.pokaInterfejs();
            int ms = 0;
            int lot = -1;
            int time = 100;
            int gener = 0;
            for (; ; )
            {
                if (radar.run < 0) b.pokaInterfejs();
                else
                {
                    radar.pokaRadar(ref lot);                           // aktualizuje i wyswietla radar
                    b.pokaListeLotow(radar, lot);                       // wypisuje liste lotow do podgledu/edycji
                    if (radar.skala > 1) radar.naniesBudynki();         // nanosi budynki na mape
                }
                if (gener == 1) radar.Losowanko();                      // generuje samoloty na mapie przez losowanie
                if (lot >= 0) b.pokaLot(radar, lot);                    // wyswietla informacje o locie
                else b.pokaKonsole(radar.skala,gener);                  // czysci konsole
                b.insert(200, 26, Convert.ToString(ms / 10 + " s"));    // wypisuje czas trwania programu
                b.insert(212, 51, Convert.ToString("."));               // wypisuje nic na koncu okna

                if (Console.KeyAvailable)                               // pobiera wybrany przycisk
                {
                    char wybor = Console.ReadKey().KeyChar;
                    if (lot >= 0) switch (wybor)
                    {
                        case '0': b.czyscKonsole(); lot = -1; break;
                        case '1': if (radar.s[lot].trajektoria < 1) radar.s[lot].trajektoria++; else radar.s[lot].trajektoria--; break;
                        case '2': radar.s[lot].zmienTrajektorie(radar.skala); break;
                    }                                        // zarzadzanie lotem
                    else if (wybor - 49 >= 0 && wybor - 49 < 10 && wybor - 49 <= radar.s.Count && radar.run>0)
                    {
                        b.czyscKonsole();
                        lot = wybor - 49;
                        if (radar.s.Count < lot + 1) lot = -1;
                    }   // wypisuje info o danym locie
                    else switch (wybor)
                    {
                        case 'q': System.Diagnostics.Process.GetCurrentProcess().Kill(); break;     // zabija aplikacje
                        case 'e': b.pokaFax(); break;                                               // info o wlascicielach
                        case 'w': losowySamolot = new Trasa(1, 2, 0, 0, 200); break;                // pusc samolot
                        case 'r': radar.run *= -1; radar.pokaSkale(); break;                        // wlaczy/wylacz radar
                        case '.': if (time > 15) time -= 15; break;                                 // przyspiesz czas
                        case ',': time += 15; break;                                                // spowolnij czas
                        case '/': time = 100; break;                                                // ustaw domyslny czas
                        case 'm': radar.zmienMape(radar.run);  break;                                        // zmienia mape
                        case 'g': if (gener == 0) gener = 1; else gener = 0; break;                 // generuje lot
                        case 'b': if (radar.skala > 1) b.pokaListeBudynkow(radar); break;           // generuje lot
                        case 'x': break;        // 
                    }
                }
                Thread.Sleep(time);
                ms++;
                if (losowySamolot.v != 0) b.pokaSamolot(ref losowySamolot); // ladujacy samolot
            }
        }
    }
}