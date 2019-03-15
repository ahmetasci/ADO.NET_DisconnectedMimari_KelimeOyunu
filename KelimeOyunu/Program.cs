using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
/*
 * 
 * */

namespace KelimeOyunu
{
    class Program
    {
        /*
        ADO.NET ile NorthWind veritabanına bağlanılacak.
        Kelimeler products tablosundan gelecek.
        Kullanıcıdan harf alınarak alınan harfle başlayan ProductName'in son harfi tanınarak son harf tekrar başka bir ProductName için
        ilk harf kabul edilip yeni bir ProductName üretilecek.
        KISITLAR:
        1.Kullanılan kelime bir daha kullanılmayacak.
        2.Eğer veritabanında birden çok kelimeden oluşan bir ProductName varsa ilk kelimesi ürün adı olarak kabul edilecek.
        */
        static void Main(string[] args)
        {
            //DISCONNECTED MIMARİ
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-61BLH0I;Initial Catalog=Northwind;Integrated Security=True");
            SqlDataAdapter adp = new SqlDataAdapter();
            adp.SelectCommand = new SqlCommand("Select case when CHARINDEX(' ',ProductName)>0 Then LEFT(ProductName, CHARINDEX(' ', ProductName) - 1) else ProductName END FROM PRODUCTS", con);
            /*
             * Charindex'le productName içinde boşluk kaçıncı indexteyse onu döndürür.
             * Left ile ProductName'de boşluktan önceki(Sol tarafını) alıcak.
             * Else durumunda ise Charindexte birşey bulamadıysa (boşluk bulamadıysa) direk kelimeyi yazdırır.

             * */
            DataTable dt = new DataTable();
            adp.Fill(dt);
            while (1 == 1)
            {


                Console.Write("Lütfen harf giriniz: ");
                string alınanharf = Console.ReadLine().ToUpper();//Kullanıcıdan alınan harf

                bool sart = true;
                int i = 0;//döngüde bakılan satır
                int x = 0;//kelime sayısı
                string tablodegeri;// tablodaki i'ye göre o an satırdaki değer

                do
                {

                    tablodegeri = dt.Rows[i][0].ToString().ToUpper();
                    //0. sütunun i. satırına bakar.
                    if (tablodegeri.Substring(0, 1) == alınanharf)
                    //Substring(0,1) 0.harften başlayıp ilk harfi alır.
                    {
                        Console.WriteLine(tablodegeri);

                        alınanharf = tablodegeri[tablodegeri.Length - 1].ToString();//alınanharf tablodeğerinin son harfine eşitledik.
                                                                                    //Burası indis mantığıyla çalışır.
                                                                                    //Örneğin ALICE ürünü için:  Length=5 
                                                                                    //alınanharf = tablodegeri[5- 1]  ===> ALICE ürünü için 4. indis E harfi yani son harf.         

                        dt.Rows[i][0] = " "; //Kullanılan ürünün olduğu satırı boş olarak tekrar yazdık.
                        //Yani kullanılan ürünü listeden silerek oyunda bir daha yazmıyoruz.
                        i = 0; //i'yi sıfırlamamızın sebebi ikinci kelimeye bakarken bütün satırlara bakabilsin
                        x++; //kelime sayısını arttırdık.
                    }

                    if (i == dt.Rows.Count - 1)// dt datatableında program satır sayısı bittiği için patlıyor.
                                               //Oyüzden satır sayısı bitmeden hemen önce döngüden çıkardık
                    {
                        sart = false;
                        Console.WriteLine("{0} kelime bulundu", x);
                    }

                    i++;
                } while (sart);
                Console.WriteLine("\n*************************************************************\n");
            }
        }
    }
}
