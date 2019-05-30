using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToDoApp503.Models
{
    public class ToDoItem :BaseEntity
    {

        [StringLength(200)]
        [Required(ErrorMessage ="Bu alan zorunludur.")]
        [DisplayName("Başlık")]
        public string Title { get; set; }
        [DisplayName("Açıklama")]
        public string Description { get; set; }
        [DisplayName("Durum")]
        public Status Status { get; set; }

        [DisplayName("Kategori")]
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [DisplayName("Kategori")]
        public virtual Category Category { get; set; }

        [StringLength(200)]
        [DisplayName("Dosya Eki")]
        public string Attachment { get; set; }

        [DisplayName("Departman")]
        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        [DisplayName("Departman")]
        public virtual Department Department { get; set; }

        [DisplayName("Taraf")]
        public int? SideId { get; set; }
        [ForeignKey("SideId")]
        [DisplayName("Taraf")]
        public virtual Side Side { get; set; }

        [DisplayName("Müşteri")]
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [DisplayName("Müşteri")]
        public virtual Customer Customer { get; set; }
        

        [DisplayName("Yönetici")]
        public int? ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        [DisplayName("Yönetici")]
        public virtual Contact Manager { get; set; }

        [DisplayName("Organizatör")]
        public int? OrganizatorId { get; set; }
        [ForeignKey("OrganizatorId")]
        [DisplayName("Organizatör")]
        public virtual Contact Organizator { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Toplantı Tarihi")]
        [Required(ErrorMessage ="Bu alan zorunludur.")]
        public DateTime MeetingDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Planlanan Tarih")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public DateTime PlannedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Bitirme Tarihi")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public DateTime FinishDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Revize Tarihi")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public DateTime ReviseDate { get; set; }

        [DisplayName("Görüşme Konusu")]
        public string ConversationSubject { get; set; }
        [DisplayName("Destekleyen Firma")]
        public string SupporterCompany { get; set; }
        [DisplayName("Destekleyen Doktor")]
        public string SupporterDoctor { get; set; }
        [DisplayName("Görüşme Katılım Sayısı")]
        [Required(ErrorMessage ="Bu alan zorunludur.")]
        public int ConversationAttendeeCount { get; set; } // Attendee=Kullanıcı

        [DisplayName("Planlanan Organizasyon Tarihi")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public DateTime ScheduleOrganizationDate { get; set; }

        [DisplayName("Mailing Konusu")]
        public string MailingSubject { get; set; }
        [DisplayName("Afiş Konusu")]
        public string PosterSubject { get; set; }
        [DisplayName("Afiş Sayısı")]
        [Required(ErrorMessage ="Bu alan zorunludur.")]
        public int PosterCount { get; set; }
        [DisplayName("Uzaktan Eğitim")]
        public string Elearning { get; set; }
        [DisplayName("Yapılan Tarama Türleri")]
        public string TypesOfScans { get; set; }
        [DisplayName("Taramalardaki ASO Sayısı")]
        public string AsoCountInScans { get; set; }
        [DisplayName("Organizasyon Türleri")]
        public string TypesOfOrganization { get; set; }
        [DisplayName("Organizasyonlardaki ASO Sayısı")]
        public string AsoCountInOrganization { get; set; }
        [DisplayName("Aşı Organizasyon Türleri")]
        public string TypeOfVaccinationOrganization { get; set; } // Vaccination = Aşı
        [DisplayName("Aşı Organizasyonunda ASO Sayısı")]
        public string AsoCountInVaccinationOrganization { get; set; }
        [DisplayName("Afiş İçin Tazminat Miktarı")]
        public string AmountOfCompensationForPoster { get; set; } // Compensation = Tazminat
        [DisplayName("Kurumsal Verimlilik Raporu")]
        public string CorporateProductivityReport { get; set; }





    }
}