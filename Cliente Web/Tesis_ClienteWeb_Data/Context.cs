using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using Tesis_ClienteWeb_Models.POCO;
using Tesis_ClienteWeb_Data.Repositories;

namespace Tesis_ClienteWeb_Data
{
    public class Context : IdentityDbContext<User>
    {
        #region Constructor
        public Context()
            : base("Context", throwIfV1Schema: false)
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }
        #endregion

        #region DbSet
        /* 
         * Estructura de datos que necesita Entity Framework Code First para poder asociar las POCO classes 
         * con el modelo de la base de datos. No borrar.
         */
        public DbSet<Assessment> Assessments { get; set; } //Asignaciones/Evaluaciones
        //public DbSet<CASU> CASUs { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Core> Cores { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Institute> Institutes { get; set; }
        public DbSet<KnowledgeArea> KnowledgeAreas { get; set; }
        public DbSet<KnowledgeSubArea> KnowledgeSubAreas { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        //public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Representative> Representatives { get; set; }
        public DbSet<Region> Regions { get; set; }
        //public DbSet<RelatedCareer> RelatedCareers { get; set; }
        new public DbSet<Role> Roles { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        //public DbSet<User> Users { get; set; } //Se utiliza el User referenciado por Identity
        #endregion

        #region On Model Creating
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Score
            //Primary Keys
            modelBuilder.Entity<Score>().HasKey(k => new { k.StudentId, k.AssessmentId });
            modelBuilder.Entity<Student>().HasMany(k => k.Scores);
            modelBuilder.Entity<Assessment>().HasMany(k => k.Scores);

            //Relationships
            modelBuilder.Entity<Score>()
                .HasRequired(t => t.Student)
                .WithMany(t => t.Scores)
                .HasForeignKey(t => t.StudentId);

            modelBuilder.Entity<Score>()
                .HasRequired(t => t.Assessment)
                .WithMany(t => t.Scores)
                .HasForeignKey(t => t.AssessmentId);
            #endregion
            #region CASU
            //Primary Keys
            modelBuilder.Entity<CASU>().HasKey(k => new { k.CourseId, k.PeriodId, k.SubjectId });
            modelBuilder.Entity<Course>().HasMany(k => k.CASUs);
            modelBuilder.Entity<Subject>().HasMany(k => k.CASUs);
            modelBuilder.Entity<Period>().HasMany(k => k.CASUs);

            modelBuilder.Entity<CASU>()
                .HasRequired(t => t.Course)
                .WithMany(t => t.CASUs)
                .HasForeignKey(t => t.CourseId);

            modelBuilder.Entity<CASU>()
                .HasRequired(t => t.Period)
                .WithMany(t => t.CASUs)
                .HasForeignKey(t => t.PeriodId);

            modelBuilder.Entity<CASU>()
                .HasRequired(t => t.Subject)
                .WithMany(t => t.CASUs)
                .HasForeignKey(t => t.SubjectId);

            modelBuilder.Entity<CASU>()
                .HasOptional(t => t.Teacher)
                .WithMany(t => t.CASUs)
                .HasForeignKey(t => t.TeacherId);
            #endregion            
            #region Opportunity
            //Primary Keys
            modelBuilder.Entity<Opportunity>().HasKey(k => new { k.CareerId, k.CoreId, k.InstituteId });
            modelBuilder.Entity<Career>().HasMany(k => k.Oportunities);
            modelBuilder.Entity<Core>().HasMany(k => k.Offers);

            modelBuilder.Entity<Opportunity>()
                .HasRequired(t => t.Career)
                .WithMany(t => t.Oportunities)
                .HasForeignKey(t => t.CareerId);

            modelBuilder.Entity<Opportunity>()
                .HasRequired(t => t.Core)
                .WithMany(t => t.Offers)
                .HasForeignKey(t => new { t.CoreId, t.InstituteId });
            #endregion
            #region RelatedCareer
            //Primary Keys
            modelBuilder.Entity<RelatedCareer>().HasKey(k => new { k.PrincipalCareerId, k.RelatedCareerId });
            modelBuilder.Entity<Career>().HasMany(k => k.PrincipalCareers);
            modelBuilder.Entity<Career>().HasMany(k => k.RelatedCareers);

            modelBuilder.Entity<RelatedCareer>()
                .HasRequired(t => t.PrincipalCareer)
                .WithMany(t => t.PrincipalCareers)
                .HasForeignKey(t => t.PrincipalCareerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RelatedCareer>()
                .HasRequired(t => t.Career)
                .WithMany(t => t.RelatedCareers)
                .HasForeignKey(t => t.RelatedCareerId)
                .WillCascadeOnDelete(false);
            #endregion
            #region Core
            //Primary Keys
            modelBuilder.Entity<Core>().HasKey(k => new { k.CoreId, k.InstituteId });
            modelBuilder.Entity<Institute>().HasMany(k => k.Cores);
            modelBuilder.Entity<Core>().HasMany(k => k.Extensions);

            //Foreign Keys
            modelBuilder.Entity<Core>()
                .HasRequired(t => t.Institute)
                .WithMany(t => t.Cores)
                .HasForeignKey(t => t.InstituteId);

            modelBuilder.Entity<Core>()
                .HasOptional(t => t.Offers);
            #endregion            
            #region SentNotification
            //Primary Keys
            modelBuilder.Entity<SentNotification>().HasKey(k => new { k.SentNotificationId, k.NotificationId });
            modelBuilder.Entity<Notification>().HasMany(k => k.SentNotifications);

            //Relationships
            modelBuilder.Entity<SentNotification>()
                .HasRequired(k => k.Notification)
                .WithMany(k => k.SentNotifications)
                .HasForeignKey(k => k.NotificationId);

            modelBuilder.Entity<SentNotification>().HasOptional(k => k.Student);
            modelBuilder.Entity<SentNotification>().HasOptional(k => k.User);
            modelBuilder.Entity<SentNotification>().HasOptional(k => k.Course);
            #endregion
            #region Event
            //Relationships
            modelBuilder.Entity<Event>()
                .HasMany(m => m.Notifications)
                .WithOptional(m => m.Event);
            #endregion
            #region ContentBlock
            //Primary Keys
            modelBuilder.Entity<ContentBlock>().HasKey(k => new { k.ContentBlockId, k.SubjectId });
            modelBuilder.Entity<Subject>().HasMany(k => k.ContentBlocks);

            //Relationships
            modelBuilder.Entity<ContentBlock>()
                .HasRequired(m => m.Subject)
                .WithMany(m => m.ContentBlocks)
                .HasForeignKey(m => m.SubjectId);
            #endregion
            #region Indicator
            //Primary Keys
            modelBuilder.Entity<Indicator>().HasKey(k => new { k.IndicatorId, k.CompetencyId });
            modelBuilder.Entity<Competency>().HasMany(k => k.Indicators);

            //Relationships
            modelBuilder.Entity<Indicator>()
                .HasRequired(m => m.Competency)
                .WithMany(m => m.Indicators)
                .HasForeignKey(m => m.CompetencyId);
            #endregion
            #region IndicatorAssessment
            //Primary Keys
            modelBuilder.Entity<IndicatorAssessment>()
                .HasKey(k => new { k.IndicatorId, k.CompetencyId, k.AssessmentId });

            modelBuilder.Entity<Indicator>().HasMany(k => k.IndicatorAssessments);
            modelBuilder.Entity<Assessment>().HasMany(k => k.IndicatorAssessments);

            //Relationships
            modelBuilder.Entity<IndicatorAssessment>()
                .HasRequired(m => m.Indicator)
                .WithMany(m => m.IndicatorAssessments)
                .HasForeignKey(m => new { m.IndicatorId, m.CompetencyId });

            modelBuilder.Entity<IndicatorAssessment>()
                .HasRequired(m => m.Assessment)
                .WithMany(m => m.IndicatorAssessments)
                .HasForeignKey(m => m.AssessmentId);
            #endregion
            #region IndicatorAssignations
            //Primary Keys
            modelBuilder.Entity<IndicatorAssignation>()
                .HasKey(k => new { k.IndicatorAssignationId, k.IndicatorId, k.CompetencyId, k.AssessmentId });
            modelBuilder.Entity<IndicatorAssessment>().HasMany(k => k.IndicatorAsignations);

            modelBuilder.Entity<IndicatorAssignation>()
                .HasRequired(m => m.IndicatorAssessment)
                .WithMany(m => m.IndicatorAsignations)
                .HasForeignKey(m => new { m.IndicatorId, m.CompetencyId, m.AssessmentId });
            #endregion
        }
        #endregion
        #region Create Context
        public static Context Create()
        {
            return new Context();
        }
        #endregion
        #region Save Changes
        public override int SaveChanges()
        {
            #region Variables para manejar excepciones
            List<string> listaErrores;
            string error;
            string excepcion;
            #endregion

            try
            {
                return base.SaveChanges();
            }

            catch (DbEntityValidationException e) //Excepción encapsulada el día: 21/10/2014
            {
                listaErrores = e.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage).ToList();

                error = string.Join("; ", listaErrores); //Se une la lista en una sola variable string
                excepcion = string.Concat(e.Message, " Los errores de validación son: ", error);

                throw new DbEntityValidationException(excepcion, e.EntityValidationErrors);

                //Fuente: http://blogs.infosupport.com/improving-dbentityvalidationexception/
            }

            catch (DbUpdateException e) //Excepción encapsulada el día: 21/10/2014
            {
                error = e.InnerException.InnerException.Message;
                excepcion = string.Concat(e.Message, " El detalle del error es: ", error);

                throw new DbUpdateException(excepcion, e);
            }

            catch (InvalidOperationException e) //Excepción encapsulada el día: 21/10/2014
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }
        #endregion
    }
}