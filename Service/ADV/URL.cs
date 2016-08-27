using Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.ADV
{
    #region models
    public class InputURLViewModels
    {
        public decimal Id { get; set; }
        public string URL { get; set; }

        public decimal Budget { get; set; }
        public decimal CategoryId { get; set; }

        public bool Status { get; set; }
        public string AutoPictureUrl { get; set; }
        public List<DeviceTypeInput> DeviceTypeInputs { get; set; }
        public List<PriceInput> PriceInputs { get; set; }

        public Nullable<decimal> DisplayCurrencyId { get; set; }
        public Nullable<decimal> DisplayLocationId { get; set; }
        public Nullable<decimal> DisplayTimeframeId { get; set; }
        public Nullable<int> DisplayTimes { get; set; }
    }
    public class DeviceTypeInput
    {
        public decimal DeviceTypeId { get; set; }
        public Nullable<decimal> ExistedDeviceTypeId { get; set; }
        public string Picture { get; set; }
        public string Title { get; set; }
        public string ExistedTitle { get; set; }
    }
    public class PriceInput
    {
        public decimal CurrencyId { get; set; }
        public Nullable<decimal> ExistedCurrencyId { get; set; }
        public Nullable<decimal> Price { get; set; }
    }
    public class CurrencyInput
    {
        public decimal CurrencyId { get; set; }
        public decimal Number { get; set; }
    }
    #endregion
    public class URL
    {
        #region private properties
        dtEntities1 db = new dtEntities1();
        #endregion
        #region private properties
        public string MessageCode { get; set; }
        public string ImageFolderPath { get; set; }
        public string TempImageFolderPath { get; set; }
        public string LoginedUser { get; set; }
        #endregion
        #region public methods
        public bool TestCreate(InputURLViewModels model)
        {
            return true;
            try
            {
                //Add URL
                //Create the nes id
                Data.URL last = db.URLs.OrderByDescending(m => m.Id).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.Id;
                }
                id++;
                //Save the URL
                Data.URL newURL = new Data.URL
                {
                    URL1 = model.URL,
                    Budget = model.Budget,
                    Status = model.Status,
                    CategoryId = model.CategoryId,
                    DisplayCurrencyId = model.DisplayCurrencyId,
                    DisplayTimeId = model.DisplayTimeframeId,
                    DisplayLocationId = model.DisplayLocationId,
                    DisplayTimes = model.DisplayTimes
                };

                newURL.Id = id;
                newURL.CreatedDate = DateTime.Now;

                db.URLs.Add(newURL);

                //Add Prices
                foreach (DeviceTypeInput device in model.DeviceTypeInputs)
                {
                    //Get the picture
                    if (string.IsNullOrEmpty(device.Picture))
                    {
                        if (!string.IsNullOrEmpty(model.AutoPictureUrl))
                        {
                            byte[] data;
                            using (WebClient client = new WebClient())
                            {
                                data = client.DownloadData(model.AutoPictureUrl);
                            }
                            string fileName = LoginedUser + "_" + device.DeviceTypeId + ".png";
                            string filePath = Path.Combine(ImageFolderPath, fileName);
                            File.WriteAllBytes(filePath, data);
                        }
                    }
                    else
                    {
                        string[] parts = device.Picture.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                        device.Picture = parts[parts.Length - 1];
                        string sourceFile = Path.Combine(TempImageFolderPath, device.Picture);
                        string destFile = Path.Combine(ImageFolderPath, device.Picture);
                        File.Copy(sourceFile, destFile, true);
                    }
                    DeviceTypeURL deviceURL = new DeviceTypeURL
                    {
                        URLId = newURL.Id,
                        DeviceTypeId = device.DeviceTypeId,
                        Title = device.Title,
                        PictureURL = device.Picture == null ? string.Empty : device.Picture
                    };
                    db.DeviceTypeURLs.Add(deviceURL);
                }

                foreach (PriceInput price in model.PriceInputs)
                {
                    db.PriceURLs.Add(new PriceURL
                    {
                        URLId = newURL.Id,
                        CurrencyId = price.CurrencyId,
                        Price = price.Price == null ? 0 : decimal.Parse(price.Price.ToString())
                    });
                }

                db.SaveChanges();
                //Clean
                last = null;
                return true;
            }
            catch (Exception error)
            {
                MessageCode = "PerformmingError";
                return false;
            }
        }
        public bool Create(InputURLViewModels model)
        {
            try
            {
                if (db.URLs.Where(m => m.URL1.ToLower().Trim() == model.URL.ToLower().Trim()).FirstOrDefault() != null)
                {
                    return false;
                }
                //Add URL
                //Create the nes id
                Data.URL last = db.URLs.OrderByDescending(m => m.Id).FirstOrDefault();
                decimal id = 0;
                if (last != null)
                {
                    id = last.Id;
                }
                id++;
                //Save the URL
                Data.URL newURL = new Data.URL
                {
                    URL1 = model.URL,
                    Budget = model.Budget,
                    Status = model.Status,
                    CategoryId = model.CategoryId,
                    DisplayCurrencyId = model.DisplayCurrencyId,
                    DisplayTimeId = model.DisplayTimeframeId,
                    DisplayLocationId = model.DisplayLocationId,
                    DisplayTimes = model.DisplayTimes
                };

                newURL.Id = id;
                newURL.CreatedDate = DateTime.Now;

                db.URLs.Add(newURL);
                db.SaveChanges();

                if (model.DeviceTypeInputs != null)
                {
                    //Add devices
                    foreach (DeviceTypeInput device in model.DeviceTypeInputs)
                    {
                        //Get the picture
                        if (string.IsNullOrEmpty(device.Picture))
                        {
                            if (!string.IsNullOrEmpty(model.AutoPictureUrl))
                            {
                                try
                                {
                                    byte[] data;
                                    using (WebClient client = new WebClient())
                                    {
                                        if (!model.AutoPictureUrl.ToLower().StartsWith("http:"))
                                        {
                                            model.AutoPictureUrl = "http:" + model.AutoPictureUrl;
                                        }
                                        data = client.DownloadData(model.AutoPictureUrl);
                                    }
                                    string fileName = device.Picture = LoginedUser + "_" + device.DeviceTypeId + ".png";
                                    string filePath = Path.Combine(ImageFolderPath, fileName);
                                    File.WriteAllBytes(filePath, data);
                                }
                                catch { device.Picture = string.Empty; }
                            }
                        }
                        else
                        {
                            string[] parts = device.Picture.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                            device.Picture = LoginedUser + "_" + parts[parts.Length - 1];
                            string sourceFile = Path.Combine(TempImageFolderPath, device.Picture);
                            string destFile = Path.Combine(ImageFolderPath, device.Picture);
                            File.Copy(sourceFile, destFile, true);
                        }
                        DeviceTypeURL deviceURL = new DeviceTypeURL
                        {
                            URLId = newURL.Id,
                            DeviceTypeId = device.DeviceTypeId,
                            Title = device.Title == null ? string.Empty : device.Title,
                            PictureURL = device.Picture == null ? string.Empty : device.Picture
                        };
                        db.DeviceTypeURLs.Add(deviceURL);
                    }
                }
                //db.SaveChanges();

                decimal priceValue = 0;
                foreach (PriceInput price in model.PriceInputs)
                {
                    if (price.Price != null)
                    {
                        priceValue = decimal.Parse(price.Price.ToString());
                    }
                    db.PriceURLs.Add(new PriceURL
                    {
                        URLId = newURL.Id,
                        CurrencyId = price.CurrencyId,
                        Price = priceValue
                    });
                }

                db.SaveChanges();
                //Clean
                last = null;
                return true;
            }
            catch (Exception error)
            {
                //MessageCode = "PerformmingError";
                MessageCode = error.Message;
                return false;
            }
        }
        public bool Update(InputURLViewModels model)
        {
            try
            {
                Data.URL url = (from m in db.URLs
                                where m.Id != model.Id
                                     & m.URL1.ToLower().Trim() == model.URL.ToLower().Trim()
                                select m).FirstOrDefault();

                if (url != null)
                {
                    return false;
                }
                //Add URL
                //Create the nes id
                url = db.URLs.Where(m => m.Id == model.Id).FirstOrDefault();
                if (url == null)
                {
                    return false;
                }
                url.URL1 = model.URL;
                url.Budget = model.Budget;
                url.Status = model.Status;
                url.DisplayCurrencyId = model.DisplayCurrencyId;
                url.DisplayLocationId = model.DisplayLocationId;
                url.DisplayTimeId = model.DisplayTimeframeId;
                url.DisplayTimes = model.DisplayTimes;

                db.SaveChanges();
                //Delete devices
                IEnumerable<Data.DeviceTypeURL> devices = db.DeviceTypeURLs.Where(m => m.URLId == model.Id);
                db.DeviceTypeURLs.RemoveRange(devices);
                //Add devices
                foreach (DeviceTypeInput deviceType in model.DeviceTypeInputs)
                {
                    //Get the picture
                    if (string.IsNullOrEmpty(deviceType.Picture))
                    {
                        if (!string.IsNullOrEmpty(model.AutoPictureUrl))
                        {
                            try
                            {
                                byte[] data;
                                using (WebClient client = new WebClient())
                                {
                                    if (!model.AutoPictureUrl.ToLower().StartsWith("http:"))
                                    {
                                        model.AutoPictureUrl = "http:" + model.AutoPictureUrl;
                                    }
                                    data = client.DownloadData(model.AutoPictureUrl);
                                }
                                string fileName = deviceType.Picture = LoginedUser + "_" + deviceType.DeviceTypeId + ".png";
                                string filePath = Path.Combine(ImageFolderPath, fileName);
                                File.WriteAllBytes(filePath, data);
                            }
                            catch { deviceType.Picture = string.Empty; }
                        }
                    }
                    else
                    {
                        string[] parts = deviceType.Picture.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                        deviceType.Picture = LoginedUser.TrimStart('+') + "_" + parts[parts.Length - 1];
                        string sourceFile = Path.Combine(TempImageFolderPath, deviceType.Picture);
                        string destFile = Path.Combine(ImageFolderPath, deviceType.Picture);
                        File.Copy(sourceFile, destFile, true);
                    }
                    DeviceTypeURL deviceURL = new DeviceTypeURL
                    {
                        URLId = model.Id,
                        DeviceTypeId = deviceType.DeviceTypeId,
                        Title = deviceType.Title == null ? string.Empty : deviceType.Title,
                        PictureURL = deviceType.Picture == null ? string.Empty : deviceType.Picture
                    };
                    db.DeviceTypeURLs.Add(deviceURL);
                }
                db.SaveChanges();
                //Delete prices
                IEnumerable<Data.PriceURL> prices = db.PriceURLs.Where(m => m.URLId == model.Id);
                db.PriceURLs.RemoveRange(prices);
                //Add prices
                decimal priceValue = 0;
                foreach (PriceInput price in model.PriceInputs)
                {
                    if (price.Price != null)
                    {
                        priceValue = decimal.Parse(price.Price.ToString());
                    }
                    db.PriceURLs.Add(new PriceURL
                    {
                        URLId = model.Id,
                        CurrencyId = price.CurrencyId,
                        Price = priceValue
                    });
                }

                db.SaveChanges();
                //Clean
                url = null;
                devices = null;
                prices = null;

                return true;
            }
            catch (Exception error)
            {
                MessageCode = "PerformmingError";
                return false;
            }
        }
        public IEnumerable<Data.URL> GetList(decimal Id, string URL, decimal categoryId)
        {
            IEnumerable<Data.URL> URLs = from u in db.URLs
                                         where 1 == 1
                                         && u.CategoryId == categoryId
                                         && (Id == 0 || Id == u.Id)
                                         && (string.IsNullOrEmpty(URL) || URL == u.URL1)
                                         orderby u.URL1
                                         select u;

            return URLs;
        }
        public IEnumerable<DeviceTypeInput> GetURLDeviceList(decimal Id, string pictureURL, decimal URLId)
        {
            IEnumerable<DeviceTypeInput> devices = from c in db.SP_GetURLAllDeviceTypes2(Id, pictureURL, URLId)
                                               select new DeviceTypeInput { DeviceTypeId = c.DeviceTypeId, ExistedDeviceTypeId = c.ExistedDeviceTypeId, Picture = c.Picture, Title = c.Title, ExistedTitle = c.ExistedTitle };

            return devices;
        }
        public IEnumerable<PriceInput> GetURLPriceList(decimal currencyId, decimal URLId)
        {
            IEnumerable<PriceInput> prices = from c in db.SP_GetURLAllPrices(currencyId, URLId)
                                             select new PriceInput { CurrencyId = c.CurrencyId, ExistedCurrencyId = c.ExistedCurrencyId, Price = c.Price };

            return prices;
        }
        public bool Update(Data.URL inputURL)
        {
            try
            {
                Data.URL URL = db.URLs.Where(s => s.Id == inputURL.Id).FirstOrDefault();
                if (URL != null)
                {
                    URL.URL1 = inputURL.URL1;
                    URL.Status = inputURL.Status;
                    URL.Budget = inputURL.Budget;

                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception error)
            {
                MessageCode = "PerformmingError";
                return false;
            }
        }
        public bool Dekete(decimal id)
        {
            try
            {
                Data.URL URL = db.URLs.Where(s => s.Id == id).FirstOrDefault();
                if (URL != null)
                {
                    db.URLs.Remove(URL);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception error)
            {
                MessageCode = "PerformmingError";
                return false;
            }
        }
        #endregion
    }
}
