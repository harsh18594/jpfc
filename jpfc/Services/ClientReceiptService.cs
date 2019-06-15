using jpfc.Classes;
using jpfc.Data.Interfaces;
using jpfc.Models;
using jpfc.Models.ClientViewModels;
using jpfc.Models.ReportViewModels;
using jpfc.Services.Interfaces;
using jpfc.Services.Reports;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class ClientReceiptService : IClientReceiptService
    {
        public readonly ILogger _logger;
        public readonly IClientReceiptRepository _clientReceiptRepository;
        public readonly IClientIdentificationRepository _clientIdentificationRepository;
        public readonly IClientRepository _clientRepository;
        public readonly IClientBelongingRepository _clientBelongingRepository;
        public readonly IHostingEnvironment _env;
        public readonly IDateTimeService _dateTimeService;

        public ClientReceiptService(ILogger<ClientReceiptService> logger,
            IClientReceiptRepository clientReceiptRepository,
            IClientIdentificationRepository clientIdentificationRepository,
            IClientRepository clientRepository,
            IClientBelongingRepository clientBelongingRepository,
            IHostingEnvironment env,
            IDateTimeService dateTimeService)
        {
            _logger = logger;
            _clientReceiptRepository = clientReceiptRepository;
            _clientIdentificationRepository = clientIdentificationRepository;
            _clientRepository = clientRepository;
            _clientBelongingRepository = clientBelongingRepository;
            _env = env;
            _dateTimeService = dateTimeService;
        }

        public async Task<(bool Success, string Error, CreateClientReceiptViewModel Model)> GetCreateClientReceiptViewModelAsync(int clientId, int? receiptId)
        {
            var success = false;
            var error = "";
            var model = new CreateClientReceiptViewModel
            {
                ClientBelongingViewModel = new ClientBelongingViewModel
                {
                    ClientReceiptId = receiptId ?? 0
                }
            };

            try
            {
                if (clientId > 0)
                {
                    model.ClientId = clientId;
                    model.Date = DateTime.Now;

                    // fetch client information
                    var client = await _clientRepository.FetchBaseByIdAsync(clientId);
                    model.ClientName = $"{client.FirstName} {client.LastName}";
                    model.ClientNumber = client.ReferenceNumber;
                    model.Address = Encryption.Decrypt(client.AddressEncrypted, client.AddressUniqueKey);
                    model.ContactNumber = Encryption.Decrypt(client.ContactNumberEncrypted, client.ContactNumberUniqueKey);

                    if (receiptId > 0)
                    {
                        var receipt = await _clientReceiptRepository.FetchBaseByIdAsync(receiptId ?? 0);
                        if (receipt != null && receipt.ClientId == clientId)
                        {
                            model.Date = receipt.Date;
                            model.ClientReceiptId = receipt.ClientReceiptId;
                            model.ReceiptNumber = receipt.ReceiptNumber;
                            model.PaymentAmount = receipt.PaymentAmount;
                            model.PaymentDate = receipt.PaymentDate;
                            model.IsPaidInterestOnly = receipt.IsPaidInterestOnly ?? false;
                            model.PaymentMethod = receipt.PaymentMethod;
                            model.ClientIdentificationId = receipt.ClientIdentificationId;

                            // convert utc to local time
                            var timeZone = _dateTimeService.FetchTimeZoneInfo(Constants.System.TimeZone);
                            model.CreatedDateTime = _dateTimeService.ConvertUtcToDateTime(receipt.CreatedUtc, timeZone);

                            success = true;
                        }
                        else
                        {
                            error = "Invalid Receipt Id.";
                        }
                    }
                    else
                    {
                        success = true;
                    }
                }
                else
                {
                    error = "Invalid Request";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";
                _logger.LogError("ClientReceiptService.GetCreateClientReceiptViewModelAsync - ex:{@Ex}", new object[] { ex });
            }

            return (success, error, model);
        }

        public async Task<(bool Success, string Error, int ReceiptId)> SaveClientReceiptAsync(CreateClientReceiptViewModel model, string userId)
        {
            var success = false;
            var error = "";
            int receiptId = 0;

            try
            {
                Models.ClientReceipt receipt = null;
                if (model.ClientReceiptId > 0)
                {
                    receipt = await _clientReceiptRepository.FetchBaseByIdAsync(model.ClientReceiptId.Value);
                }
                if (receipt == null)
                {
                    // save reference number for new records
                    var maxReceiptId = await _clientReceiptRepository.GetMaxReceiptIdAsync();
                    var receiptNumber = $"RE{DateTime.Now.ToString("yyyyMMdd")}{maxReceiptId + 1}";

                    receipt = new Models.ClientReceipt
                    {
                        CreatedUserId = userId,
                        CreatedUtc = DateTime.UtcNow,
                        // assign date and client once only when the receipt is created, do not allow modifying them
                        ClientId = model.ClientId,
                        Date = model.Date,
                        ReceiptNumber = receiptNumber
                    };
                }
                else
                {
                    receipt.AuditUserId = userId;
                    receipt.AuditUtc = DateTime.UtcNow;
                }

                // if saved id is selected, use that, else create a new one
                if (model.ClientIdentificationId.HasValue)
                {
                    receipt.ClientIdentificationId = model.ClientIdentificationId.Value;
                }
                else
                {
                    // save client id first and assign it to receipt
                    // encrypt id info
                    var idNumberEncryptionResult = Encryption.Encrypt(model.IdentificationDocumentNumber);
                    var clientIdentification = new ClientIdentification
                    {
                        ClientId = model.ClientId,
                        IdentificationDocumentId = model.IdentificationDocumentId,
                        IdentificationDocumentNumberEncrypted = idNumberEncryptionResult.EncryptedString,
                        IdentificaitonDocumentNumberUniqueKey = idNumberEncryptionResult.UniqueKey,
                        CreatedUserId = userId,
                        CreatedUtc = DateTime.UtcNow
                    };
                    await _clientIdentificationRepository.SaveClientIdentificationAsync(clientIdentification);

                    receipt.ClientIdentificationId = clientIdentification.ClientIdentificationId;
                }

                receipt.PaymentDate = model.PaymentDate;
                receipt.PaymentAmount = model.PaymentAmount;
                receipt.IsPaidInterestOnly = model.IsPaidInterestOnly;
                receipt.PaymentMethod = model.PaymentMethod;

                // save to database
                await _clientReceiptRepository.SaveClientReceiptAsync(receipt);
                receiptId = receipt.ClientReceiptId;
                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";
                _logger.LogError("ClientReceiptService.SaveClientReceiptAsync - ex:{@Ex}", new object[] { ex });
            }

            return (success, error, receiptId);
        }

        public async Task<(bool Success, string Error, ICollection<ClientReceiptViewModel> Model)> ListClientReceiptAsync(int clientId)
        {
            var success = false;
            var error = "";
            ICollection<ClientReceiptViewModel> model = new List<ClientReceiptViewModel>();

            try
            {
                model = await _clientReceiptRepository.ListByClientIdAsync(clientId);
                // convert utc to local time
                var timeZone = _dateTimeService.FetchTimeZoneInfo(Constants.System.TimeZone);
                foreach (var item in model)
                {
                    item.Date = _dateTimeService.ConvertUtcToDateTime(item.CreatedUtc, timeZone);
                }
                success = true;
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";
                _logger.LogError("ClientReceiptService.ListClientReceiptAsync - ex:{@Ex}", new object[] { ex });
            }

            return (success, error, model);
        }

        public async Task<(bool Success, string Error)> DeleteClientReceiptByIdAsync(int receiptId)
        {
            var success = false;
            var error = "";

            try
            {
                if (receiptId > 0)
                {
                    var receipt = await _clientReceiptRepository.FetchFullByIdAsync(receiptId);
                    if (receipt != null)
                    {
                        if (receipt.ClientBelongings?.Any() == false)
                        {
                            await _clientReceiptRepository.DeleteClientReceiptAsync(receipt);
                            success = true;
                        }
                        else
                        {
                            error = $"Unable to delete. Receipt has {receipt.ClientBelongings.Count} belonging(s).";
                        }
                    }
                    else
                    {
                        error = "Unable to locate receipt information";
                    }
                }
                else
                {
                    error = "Invalid Request";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";
                _logger.LogError("ClientReceiptService.DeleteClientReceiptByIdAsync - ex:{@Ex}", new object[] { ex });
            }

            return (success, error);
        }

        public async Task<(bool Success, string Error, int ReceiptId)> DuplicateClientReceiptByIdAsync(int receiptId, string userId)
        {
            var success = false;
            var error = "";
            int duplicateReceiptId = 0;

            try
            {
                if (receiptId > 0)
                {
                    var originalReceipt = await _clientReceiptRepository.FetchFullByIdAsync(receiptId);
                    if (originalReceipt != null)
                    {
                        // duplicate receipt info
                        var maxReceiptId = await _clientReceiptRepository.GetMaxReceiptIdAsync();
                        var receiptNumber = $"RE{DateTime.Now.ToString("yyyyMMdd")}{maxReceiptId + 1}";

                        var duplicateReceipt = new Models.ClientReceipt
                        {
                            CreatedUserId = userId,
                            CreatedUtc = DateTime.UtcNow,
                            ClientId = originalReceipt.ClientId,
                            Date = DateTime.Now.Date,
                            ReceiptNumber = receiptNumber,
                            ClientIdentificationId = originalReceipt.ClientIdentificationId,
                            PaymentDate = null,
                            PaymentAmount = null,
                            IsPaidInterestOnly = null,
                            PaymentMethod = null
                        };

                        // copy belongings
                        var originalBelongings = originalReceipt.ClientBelongings;
                        if (originalBelongings?.Any() == true)
                        {
                            foreach (var item in originalBelongings)
                            {
                                var duplicateBelonging = new ClientBelonging
                                {
                                    CreatedUserId = userId,
                                    CreatedUtc = DateTime.UtcNow,
                                    FinalPrice = item.FinalPrice,
                                    ItemPrice = item.ItemPrice,
                                    ItemWeight = item.ItemWeight,
                                    KaratId = item.KaratId,
                                    KaratOther = item.KaratOther,
                                    MetalId = item.MetalId,
                                    MetalOther = item.MetalOther,
                                    ReplacementValue = item.ReplacementValue,
                                    TransactionAction = item.TransactionAction
                                };
                                duplicateReceipt.ClientBelongings.Add(duplicateBelonging);
                            }
                        }

                        // save receipt to database
                        await _clientReceiptRepository.SaveClientReceiptAsync(duplicateReceipt);

                        // return values
                        duplicateReceiptId = duplicateReceipt.ClientReceiptId;
                        success = true;
                    }
                    else
                    {
                        error = "Unable to locate receipt information";
                    }
                }
                else
                {
                    error = "Invalid Request";
                }
            }
            catch (Exception ex)
            {
                error = "Unexpected error occurred while processing your request.";
                _logger.LogError("ClientReceiptService.DuplicateClientReceiptByIdAsync - ex:{@Ex}", new object[] { ex });
            }

            return (success, error, duplicateReceiptId);
        }

        public async Task<(bool Success, string Error, byte[] FileBytes, string FileName)> ExportReceiptByReceiptIdAsync(int clientReceiptId)
        {
            var success = false;
            var error = "";
            byte[] resultBytes = null;
            var fileName = "";
            byte[] receiptBytes = null;
            byte[] loanScheduleBytes = null;

            try
            {
                var extension = "pdf";

                // fetch client info
                var receiptInfo = await _clientReceiptRepository.FetchFullByIdAsync(clientReceiptId);
                if (receiptInfo != null && receiptInfo.Client != null)
                {
                    // fetch timezone for conversion
                    var timeZone = _dateTimeService.FetchTimeZoneInfo(Constants.System.TimeZone);
                    var receiptDate = _dateTimeService.ConvertUtcToDateTime(receiptInfo.CreatedUtc, timeZone);

                    // prepare belonging list
                    var belongingsList = await _clientBelongingRepository.ListClientBelongingByReceiptIdAsync(clientReceiptId);
                    decimal clientPays = 0;
                    decimal clientGets = 0;
                    decimal billAmount = 0;
                    decimal loanAmount = 0;
                    bool clientPaysFinal = false;

                    if (belongingsList?.Any() == true)
                    {
                        // order by name to print in receipt
                        belongingsList = belongingsList.OrderBy(b => b.Metal).ToList();
                        foreach (var item in belongingsList)
                        {
                            // calculate bill amount
                            if (item.BusinessGetsMoney)
                            {
                                clientPays += item.FinalPrice ?? 0;
                            }
                            if (item.BusinessPaysMoney)
                            {
                                clientGets += item.FinalPrice ?? 0;
                            }
                            billAmount = Math.Abs(clientPays - clientGets);
                            clientPaysFinal = clientPays > clientGets;

                            // determine total loan amount from receipt
                            if (item.TransactionAction == Constants.TransactionAction.Loan)
                            {
                                loanAmount += item.FinalPrice ?? 0;
                            }
                        }
                    }

                    // prepare viewmodel
                    ReceiptViewModel model = new ReceiptViewModel
                    {
                        BillDate = receiptDate,
                        ClientNumber = receiptInfo.Client.ReferenceNumber,
                        ReceiptNumber = receiptInfo.ReceiptNumber,
                        ClientName = $"{receiptInfo.Client.FirstName} {receiptInfo.Client.LastName}",
                        Address = Encryption.Decrypt(receiptInfo.Client.AddressEncrypted, receiptInfo.Client.AddressUniqueKey),
                        ContactNumber = Classes.Helper.FormatPhoneNumber(Encryption.Decrypt(receiptInfo.Client.ContactNumberEncrypted, receiptInfo.Client.ContactNumberUniqueKey)),
                        EmailAddress = receiptInfo.Client.EmailAddress,
                        BillAmount = billAmount,
                        ClientPaysFinal = clientPaysFinal,
                        Belongings = belongingsList
                    };

                    // generate receipt
                    var pdfReceipt = new Reports.ClientReceipt(model, _env.WebRootPath);

                    // Create the document using MigraDoc.
                    var pdfReceiptDocument = pdfReceipt.CreateDocument();
                    pdfReceiptDocument.UseCmykColor = true;

                    // Create a renderer for PDF that uses Unicode font encoding.
                    var pdfRenderer = new PdfDocumentRenderer(true)
                    {
                        // Set the MigraDoc document.
                        Document = pdfReceiptDocument
                    };

                    // Create the PDF document.
                    pdfRenderer.RenderDocument();

                    // Save the receipt document...
                    using (var stream = new MemoryStream())
                    {
                        pdfRenderer.Save(stream, false);
                        receiptBytes = stream.ToArray();
                    }

                    // assign receipt bytes to result in case there is no loan schedule to add
                    resultBytes = receiptBytes;

                    // generate loan schedule and zip multiple documents, if required
                    if (loanAmount > 0)
                    {
                        var pdfLoanSchedule = new LoanSchedule(billDate: receiptDate, clientNumber: receiptInfo.Client.ReferenceNumber, receiptNumber: receiptInfo.ReceiptNumber,
                        clientName: $"{receiptInfo.Client.FirstName} {receiptInfo.Client.LastName}",
                        clientAddress: Encryption.Decrypt(receiptInfo.Client.AddressEncrypted, receiptInfo.Client.AddressUniqueKey),
                        phoneNumber: Classes.Helper.FormatPhoneNumber(Encryption.Decrypt(receiptInfo.Client.ContactNumberEncrypted, receiptInfo.Client.ContactNumberUniqueKey)),
                        emailAddress: receiptInfo.Client.EmailAddress, rootPath: _env.WebRootPath, loanAmount: loanAmount);

                        // Create the document using MigraDoc.
                        var pdfLoanScheduleDocument = pdfLoanSchedule.CreateDocument();
                        pdfLoanScheduleDocument.UseCmykColor = true;

                        // Create a renderer for PDF that uses Unicode font encoding.
                        pdfRenderer = new PdfDocumentRenderer(true)
                        {
                            // Set the MigraDoc document.
                            Document = pdfLoanScheduleDocument
                        };

                        // Create the PDF document.
                        pdfRenderer.RenderDocument();

                        // Save the loan schedule document...
                        using (var stream = new MemoryStream())
                        {
                            pdfRenderer.Save(stream, false);
                            loanScheduleBytes = stream.ToArray();
                        }

                        // zip both documents
                        using (var compressedFileStream = new MemoryStream())
                        {
                            //Create an archive and store the stream in memory.
                            using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                            {
                                // add receipt
                                if (receiptBytes != null)
                                {
                                    var receiptEntry = zipArchive.CreateEntry($"{receiptInfo.Client.FirstName}_{receiptInfo.Client.LastName}_{receiptInfo.ReceiptNumber}_Receipt.pdf");

                                    //Get the stream of the attachment
                                    using (var receiptStream = new MemoryStream(receiptBytes))
                                    {
                                        using (var zipEntryStream = receiptEntry.Open())
                                        {
                                            //Copy the attachment stream to the zip entry stream
                                            receiptStream.CopyTo(zipEntryStream);
                                        }
                                    }
                                }

                                // add loan schedule
                                if (loanScheduleBytes != null)
                                {
                                    var loanScheduleEntry = zipArchive.CreateEntry($"{receiptInfo.Client.FirstName}_{receiptInfo.Client.LastName}_{receiptInfo.ReceiptNumber}_LoanSchedule.pdf");

                                    //Get the stream of the attachment
                                    using (var loanScheduleStream = new MemoryStream(loanScheduleBytes))
                                    {
                                        using (var zipEntryStream = loanScheduleEntry.Open())
                                        {
                                            //Copy the attachment stream to the zip entry stream
                                            loanScheduleStream.CopyTo(zipEntryStream);
                                        }
                                    }
                                }
                            }
                            // override file bytes with zip file
                            resultBytes = compressedFileStream.ToArray();
                            extension = "zip";
                        }
                    }

                    fileName = $"{receiptInfo.Client.FirstName}_{receiptInfo.Client.LastName}_{receiptInfo.ReceiptNumber}_Receipt.{extension}";
                    success = true;
                }
                else
                {
                    error = "Unable to locate receipt information";
                }

            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.ExportReceiptByReceiptIdAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, FileBytes: resultBytes, FileName: fileName);
        }

        public async Task<(bool Success, string Error, byte[] FileBytes, string FileName)> ExportLoanScheduleByReceiptIdAsync(int clientReceiptId)
        {
            var success = false;
            var error = "";
            byte[] fileBytes = null;
            var fileName = "";

            try
            {
                // fetch client info
                var receiptInfo = await _clientReceiptRepository.FetchFullByIdAsync(clientReceiptId);
                if (receiptInfo != null && receiptInfo.Client != null)
                {
                    // fetch timezone for conversion
                    var timeZone = _dateTimeService.FetchTimeZoneInfo(Constants.System.TimeZone);
                    var receiptDate = _dateTimeService.ConvertUtcToDateTime(receiptInfo.CreatedUtc, timeZone);

                    // prepare belonging list
                    var belongingsList = await _clientBelongingRepository.ListClientBelongingByReceiptIdAsync(clientReceiptId);
                    decimal loanAmount = 0;

                    if (belongingsList?.Any() == true)
                    {
                        // order by name
                        belongingsList = belongingsList.OrderBy(b => b.Metal).ToList();
                        foreach (var item in belongingsList)
                        {
                            if (item.TransactionAction == Constants.TransactionAction.Loan)
                            {
                                loanAmount += item.FinalPrice ?? 0;
                            }
                        }
                    }

                    var pdfLoanSchedule = new LoanSchedule(billDate: receiptDate, clientNumber: receiptInfo.Client.ReferenceNumber, receiptNumber: receiptInfo.ReceiptNumber,
                        clientName: $"{receiptInfo.Client.FirstName } {receiptInfo.Client.LastName}",
                        clientAddress: Encryption.Decrypt(receiptInfo.Client.AddressEncrypted, receiptInfo.Client.AddressUniqueKey),
                        phoneNumber: Classes.Helper.FormatPhoneNumber(Encryption.Decrypt(receiptInfo.Client.ContactNumberEncrypted, receiptInfo.Client.ContactNumberUniqueKey)),
                        emailAddress: receiptInfo.Client.EmailAddress, rootPath: _env.WebRootPath, loanAmount: loanAmount);

                    // Create the document using MigraDoc.
                    var pdfLoanScheduleDocument = pdfLoanSchedule.CreateDocument();
                    pdfLoanScheduleDocument.UseCmykColor = true;

                    // Create a renderer for PDF that uses Unicode font encoding.
                    var pdfRenderer = new PdfDocumentRenderer(true);

                    // Set the MigraDoc document.
                    pdfRenderer.Document = pdfLoanScheduleDocument;

                    // Create the PDF document.
                    pdfRenderer.RenderDocument();

                    // Save the loan schedule document...
                    using (var stream = new MemoryStream())
                    {
                        pdfRenderer.Save(stream, false);
                        fileBytes = stream.ToArray();
                    }

                    fileName = $"{receiptInfo.Client.FirstName}_{receiptInfo.Client.LastName}_{receiptInfo.ReceiptNumber}_LoanSchedule.pdf";
                    success = true;
                }
                else
                {
                    error = "Unable to locate receipt information";
                }

            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.ExportLoanScheduleByReceiptIdAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, FileBytes: fileBytes, FileName: fileName);
        }

        public async Task<(bool Success, string Error, byte[] FileBytes, string FileName)> ExportPaymentReceiptByReceiptIdAsync(int clientReceiptId)
        {
            var success = false;
            var error = "";
            byte[] fileBytes = null;
            var fileName = "";

            try
            {
                // fetch client info
                var receiptInfo = await _clientReceiptRepository.FetchFullByIdAsync(clientReceiptId);
                if (receiptInfo != null && receiptInfo.Client != null)
                {
                    // fetch timezone for conversion
                    var timeZone = _dateTimeService.FetchTimeZoneInfo(Constants.System.TimeZone);
                    var receiptDate = _dateTimeService.ConvertUtcToDateTime(receiptInfo.CreatedUtc, timeZone);
                    var now = _dateTimeService.ConvertUtcToDateTime(DateTime.UtcNow, timeZone);

                    // prepare belonging list
                    var belongingsList = await _clientBelongingRepository.ListClientBelongingByReceiptIdAsync(clientReceiptId);
                    decimal principalLoanAmount = 0;
                    decimal loanDueAmount = 0;
                    decimal totalPurchase = 0;
                    decimal totalSell = 0;

                    if (belongingsList?.Any() == true)
                    {
                        // order by name to print in receipt
                        belongingsList = belongingsList.OrderBy(b => b.Metal).ToList();
                        foreach (var item in belongingsList)
                        {
                            // determine total loan amount from receipt
                            if (item.TransactionAction == Constants.TransactionAction.Loan)
                            {
                                principalLoanAmount += item.FinalPrice ?? 0;
                            }

                            // determine total purchase amount from receipt
                            if (item.TransactionAction == Constants.TransactionAction.Sell)
                            {
                                // if business sells an item, it is purchase for client
                                totalPurchase += item.FinalPrice ?? 0;
                            }

                            // determine total sell amount from receipt
                            if (item.TransactionAction == Constants.TransactionAction.Purchase)
                            {
                                // if business purchases an item, it is sell for client
                                totalSell += item.FinalPrice ?? 0;
                            }
                        }
                    }

                    // calculate interest rate on loan amount
                    loanDueAmount = principalLoanAmount;
                    for (var i = receiptDate; i < now; i = i.AddMonths(1))
                    {
                        // prepare due amount
                        loanDueAmount = loanDueAmount * (decimal)1.04;
                    }

                    // prepare viewmodel
                    PaymentReceiptViewModel model = new PaymentReceiptViewModel
                    {
                        BillDate = receiptDate,
                        ClientNumber = receiptInfo.Client.ReferenceNumber,
                        ReceiptNumber = receiptInfo.ReceiptNumber,
                        ClientName = $"{receiptInfo.Client.FirstName} {receiptInfo.Client.LastName}",
                        Address = Encryption.Decrypt(receiptInfo.Client.AddressEncrypted, receiptInfo.Client.AddressUniqueKey),
                        ContactNumber = Classes.Helper.FormatPhoneNumber(Encryption.Decrypt(receiptInfo.Client.ContactNumberEncrypted, receiptInfo.Client.ContactNumberUniqueKey)),
                        EmailAddress = receiptInfo.Client.EmailAddress,
                        //BillAmount = Math.Abs(totalPurchase - (principalLoanAmount + totalSell)),
                        //ClientPaysFinal = totalPurchase > (principalLoanAmount + totalSell),
                        InterestAmount = Math.Abs(loanDueAmount - principalLoanAmount),
                        PaymentMethod = receiptInfo.PaymentMethod ?? "N/A",
                        PrincipalLoanAmount = principalLoanAmount,
                        PurchaseTotal = totalPurchase,
                        SellTotal = totalSell,
                        ServiceFee = 0,
                        StorageFee = 0,
                        //BrokerageFee = 0,
                        //RetainerFee = 0,
                        Belongings = belongingsList,
                        PaymentReceived = receiptInfo.PaymentAmount ?? 0
                    };
                    // calculate final total
                    model.FinalTotal = (model.PrincipalLoanAmount + model.PurchaseTotal + model.InterestAmount + model.ServiceFee 
                                        + model.StorageFee /*+ model.BrokerageFee + model.RetainerFee*/) - model.SellTotal;

                    var paymentReceipt = new PaymentReceipt(model, _env.WebRootPath);

                    // Create the document using MigraDoc.
                    var pdfPaymentReceiptDocument = paymentReceipt.CreateDocument();
                    pdfPaymentReceiptDocument.UseCmykColor = true;

                    // Create a renderer for PDF that uses Unicode font encoding.
                    var pdfRenderer = new PdfDocumentRenderer(true);

                    // Set the MigraDoc document.
                    pdfRenderer.Document = pdfPaymentReceiptDocument;

                    // Create the PDF document.
                    pdfRenderer.RenderDocument();

                    // Save the loan schedule document...
                    using (var stream = new MemoryStream())
                    {
                        pdfRenderer.Save(stream, false);
                        fileBytes = stream.ToArray();
                    }

                    fileName = $"{receiptInfo.Client.FirstName}_{receiptInfo.Client.LastName}_{receiptInfo.ReceiptNumber}_PaymentReceipt.pdf";
                    success = true;
                }
                else
                {
                    error = "Unable to locate receipt information";
                }

            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.ExportPaymentReceiptByReceiptIdAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, FileBytes: fileBytes, FileName: fileName);
        }

        public async Task<(bool Success, string Error, AmountSummaryViewModel Model)> FetchReceiptSummaryAsync(int clientReceiptId)
        {
            var success = false;
            var error = "";
            var model = new AmountSummaryViewModel();

            try
            {
                if (clientReceiptId > 0)
                {
                    // fetch client info
                    var receiptInfo = await _clientReceiptRepository.FetchFullByIdAsync(clientReceiptId);

                    decimal principalLoanAmount = 0;
                    decimal loanDueAmount = 0;
                    decimal totalPurchase = 0;
                    decimal totalSell = 0;

                    //var belongings = await _clientBelongingRepository.ListClientBelongingByReceiptIdAsync(clientReceiptId);
                    if (receiptInfo.ClientBelongings != null && receiptInfo.ClientBelongings.Any())
                    {
                        foreach (var item in receiptInfo.ClientBelongings)
                        {
                            // determine total loan amount from receipt
                            if (item.TransactionAction == Constants.TransactionAction.Loan)
                            {
                                principalLoanAmount += item.FinalPrice ?? 0;
                            }

                            // determine total purchase amount from receipt
                            if (item.TransactionAction == Constants.TransactionAction.Purchase)
                            {
                                totalPurchase += item.FinalPrice ?? 0;
                            }

                            // determine total sell amount from receipt
                            if (item.TransactionAction == Constants.TransactionAction.Sell)
                            {
                                totalSell += item.FinalPrice ?? 0;
                            }
                        }

                        // calculate interest rate on loan amount
                        var timeZone = _dateTimeService.FetchTimeZoneInfo(Constants.System.TimeZone);
                        var receiptDate = _dateTimeService.ConvertUtcToDateTime(receiptInfo.CreatedUtc, timeZone);
                        var now = _dateTimeService.ConvertUtcToDateTime(DateTime.UtcNow, timeZone);
                        loanDueAmount = principalLoanAmount;
                        for (var i = receiptDate; i < now; i = i.AddMonths(1))
                        {
                            // prepare due amount
                            loanDueAmount = loanDueAmount * (decimal)1.04;
                        }

                        model.InterestAmount = Math.Abs(loanDueAmount - principalLoanAmount);
                        model.PrincipalLoanAmount = principalLoanAmount;
                        model.PurchaseTotal = totalPurchase;
                        model.SellTotal = totalSell;
                        model.FinalTotal = (model.PrincipalLoanAmount + model.SellTotal + model.InterestAmount + model.ServiceFee + model.StorageFee) - model.PurchaseTotal;
                    }

                    success = true;
                }
                else
                {
                    error = "Invalid request";
                }
            }
            catch (Exception ex)
            {
                error = "Somethong went wrong while processing your request.";
                _logger.LogError("ClientService.FetchRecipetSummaryAsync - exception:{@Ex}", args: new object[] { ex });
            }

            return (Success: success, Error: error, Model: model);
        }
    }
}
