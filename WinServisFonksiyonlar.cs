using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace WinServisApp
{
    class WinServisFonksiyonlar
    {
        // ServiceController[] Servisler;
        ServiceController ServisKontroleri;
        public bool ServisVarmi(string servisAdi)
        {
            // Kurulu servislerin listesi
            ServiceController[] servisler = ServiceController.GetServices();

            // servis var mı diye kontrol ediyoruz
            foreach (ServiceController servis in servisler)
            {
                if (servis.ServiceName == servisAdi)
                    return true;
            }
            return false;
        }
        public bool ServisiYukle(string servisAdi, string servisDosyaYolu)
        {
            try
            {
                if (this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et varsa işleme devam etme
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE KURULUDUR, YÜKLEME İŞLEMİNE DEVAM EDİLEMEZ !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                System.Configuration.Install.ManagedInstallerClass
                            .InstallHelper(new string[] { servisDosyaYolu });
                this.ServisiYenile(servisAdi);
                return true;
               
            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİS YÜKLEME İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        public bool ServisiSil(string servisAdi, string servisDosyaYolu)
        {
            try
            {
                if (!this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et yoksa işleme devam etme
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE BULUNAMAMIŞTIR !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                System.Configuration.Install.ManagedInstallerClass
                            .InstallHelper(new string[] { "/u", servisDosyaYolu });
                return true;

            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİSİ SİLME İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        public bool ServisiCalisiyorMu(string servisAdi)
        {
            try
            {
                if (!this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et yoksa işleme devam etme
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE BULUNAMAMIŞTIR !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                ServisKontroleri = new ServiceController(servisAdi);
                if ((ServisKontroleri.Status.Equals(ServiceControllerStatus.Running)) ||
                   (ServisKontroleri.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    return true;
                }
                return false;
            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİSİN ÇALIŞMA DURUMUNU KONTROL İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        public bool ServisiBaslat(string servisAdi)
        {
            try
            {
                if (!this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et yoksa işleme devam etme
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE BULUNAMAMIŞTIR !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                ServisKontroleri = new ServiceController(servisAdi);
                if (ServisKontroleri.Status.Equals(ServiceControllerStatus.Stopped) ||
                   ServisKontroleri.Status.Equals(ServiceControllerStatus.StopPending))
                {
                    ServisKontroleri.Start();
                    ServisKontroleri.WaitForStatus(ServiceControllerStatus.Running);
                    ServisKontroleri.Refresh();
                    return true;
                }
                return false;
            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİSİ BAŞLATMA İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        public bool ServisiDurdur(string servisAdi)
        {
            try
            {
                if (!this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et yoksa işleme devam etme 
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE BULUNAMAMIŞTIR !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                ServisKontroleri = new ServiceController(servisAdi);
                if ((ServisKontroleri.Status.Equals(ServiceControllerStatus.Running)) ||
                (ServisKontroleri.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    ServisKontroleri.Stop();
                    ServisKontroleri.WaitForStatus(ServiceControllerStatus.Stopped);
                    ServisKontroleri.Refresh();
                    return true;
                }
                return false;
            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİSİ DURDURMA İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        public bool ServisiYenidenBaslat(string servisAdi)
        {
            try
            {
                if (!this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et yoksa işleme devam etme 
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE BULUNAMAMIŞTIR !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                ServisKontroleri = new ServiceController(servisAdi);


                // kapalı ise
                if (ServisKontroleri.Status.Equals(ServiceControllerStatus.Stopped) ||
                   ServisKontroleri.Status.Equals(ServiceControllerStatus.StopPending))
                {
                    ServisKontroleri.Start();
                    ServisKontroleri.WaitForStatus(ServiceControllerStatus.Running);
                    ServisKontroleri.Refresh();
                    return true;
                }
                // açık ise 
                else if ((ServisKontroleri.Status.Equals(ServiceControllerStatus.Running)) ||
                        (ServisKontroleri.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    ServisKontroleri.Stop();
                    ServisKontroleri.WaitForStatus(ServiceControllerStatus.Stopped);
                    ServisKontroleri.Refresh();

                    ServisKontroleri.Start();
                    ServisKontroleri.WaitForStatus(ServiceControllerStatus.Running);
                    ServisKontroleri.Refresh();
                    return true;
                }

                return false;                
            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİSİ YENİDEN BAŞLATMA İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        public bool ServisiDuraklat(string servisAdi)
        {
            try
            {
                if (!this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et yoksa işleme devam etme 
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE BULUNAMAMIŞTIR !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                ServisKontroleri = new ServiceController(servisAdi);
                
                // servis çalışıyor ise 
                if ((ServisKontroleri.Status.Equals(ServiceControllerStatus.Running)) ||
                        (ServisKontroleri.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    // servis duraklatılmadı ise
                    if ((!ServisKontroleri.Status.Equals(ServiceControllerStatus.Paused)) ||
                        (!ServisKontroleri.Status.Equals(ServiceControllerStatus.PausePending)))
                    {
                        if (ServisKontroleri.CanPauseAndContinue == true) // CanPauseAndContinue true ise, Pause işlemini uygulama hakkına sahibiz.
                        {
                            ServisKontroleri.Pause(); // Servis duraksatılıyor.
                            ServisKontroleri.WaitForStatus(ServiceControllerStatus.Paused); // Servisin Paused konumuna geçmesi için bekleniyor.
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("SERVİSİ DURAKLATMA YETKİSİ YOK !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        } 
                    }
                }

                return false;
            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİSİ DURAKLATMA İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        public bool ServisiDevamEttir(string servisAdi)
        {
            try
            {
                if (!this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et yoksa işleme devam etme 
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE BULUNAMAMIŞTIR !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                ServisKontroleri = new ServiceController(servisAdi);

                // servis duraklatıldı ise
                if ((ServisKontroleri.Status.Equals(ServiceControllerStatus.Paused)) ||
                    (ServisKontroleri.Status.Equals(ServiceControllerStatus.PausePending)))
                {

                    if (ServisKontroleri.CanPauseAndContinue == true) // CanPauseAndContinue true ise, Continue işlemini uygulama hakkına sahibiz.
                    {
                        ServisKontroleri.Continue(); // Servis duraksatılıyor.
                        ServisKontroleri.WaitForStatus(ServiceControllerStatus.Running); // Servisin Running konumuna geçmesi için bekleniyor.
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("SERVİSİ DEVAM ETTİRME YETKİSİ YOK !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }                    
                }

                return false;
            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİSİ DEVAM ETTİRME İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        public bool ServisiYenile(string servisAdi)
        {
            try
            {
                if (!this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et yoksa işleme devam etme 
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE BULUNAMAMIŞTIR !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                ServisKontroleri = new ServiceController(servisAdi);
                ServisKontroleri.Refresh();
                return true;
            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİSİ YENİLEME İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        public string ServisDurumuCek(string servisAdi)
        {
            try
            {
                if (!this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et yoksa işleme devam etme 
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE BULUNAMAMIŞTIR !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "No";
                }
                else
                {
                    ServisKontroleri = new ServiceController(servisAdi);
                    return ServisKontroleri.Status.ToString();
                }
                
            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİSİ YENİDEN BAŞLATMA İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "No";
            }
        }
        public void ServiseBagliServisler(string servisAdi,ListBox list)
        {
            try
            {
                if (!this.ServisVarmi(servisAdi))
                { // servis varmı kontrol et yoksa işleme devam etme 
                    MessageBox.Show("İLGİLİ SERVİS WİNDOSWS HİZMETLERİNDE BULUNAMAMIŞTIR !!!", "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ServisKontroleri = new ServiceController(servisAdi);
                ServiceController[] bagliServisler = ServisKontroleri.ServicesDependedOn; // Bir servise bağlı servislerin listesini elde etmek için, güncel ServiceController nesnesinin, ServiceDependedOn özelliği kullanılır.

                // Bağlı servisleri Listbox'a ekle
                foreach (ServiceController sc in bagliServisler)
                {
                     list.Items.Add(sc.ServiceName.ToString());
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show("SERVİSİ YENİDEN BAŞLATMA İŞLEMİNDE BİLİNMEYEN BİR HATA OLUŞTU !!! \n\n" + "HATA İÇERİĞİ : \n " + hata.Message, "DİKKAT !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
