using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using Tesis_ClienteWeb_Models.POCO;

namespace Tesis_ClienteWeb_Data.Repositories
{
    public class UnitOfWork : IDisposable
    {
        #region Variables declaradas
        private Context contexto = new Context();
        private bool disposed = false;
        #endregion
        #region Variables repositorios
        private RepositoryBase<Assessment> repositorioAssessment;
        private RepositoryBase<Career> repositorioCareer;
        private RepositoryBase<Core> repositorioCore;
        private RepositoryBase<Country> repositorioCountry;
        private RepositoryBase<Course> repositorioCourse;
        private RepositoryBase<CASU> repositorioCASU;
        private RepositoryBase<Event> repositorioEvent;
        private RepositoryBase<Institute> repositorioInstitute;
        private RepositoryBase<KnowledgeArea> repositorioKnowledgeArea;
        private RepositoryBase<KnowledgeSubArea> repositorioKnowledgeSubArea;
        private RepositoryBase<Notification> repositorioNotification;               
        private RepositoryBase<Opportunity> repositorioOpportunity;
        private RepositoryBase<Period> repositorioPeriod;
        private RepositoryBase<Profile> repositorioProfile;
        private RepositoryBase<Region> repositorioRegion;
        private RepositoryBase<RelatedCareer> repositorioRelatedCareer;
        private RepositoryBase<Representative> repositorioRepresentative;
        private RepositoryBase<Role> repositorioRole;
        private RepositoryBase<School> repositorioSchool;
        private RepositoryBase<SchoolYear> repositorioSchoolYear;
        private RepositoryBase<Score> repositorioScore;
        private RepositoryBase<SentNotification> repositorioSentNotification;
        private RepositoryBase<State> repositorioState;
        private RepositoryBase<Student> repositorioStudent;
        private RepositoryBase<Subject> repositorioSubject;
        private RepositoryBase<User> repositorioUser;        
        #endregion
        #region Variables de inicialiazción de repositorios
        public RepositoryBase<Assessment> RepositorioAssessment
        {
            get
            {

                if (this.repositorioAssessment == null)
                {
                    this.repositorioAssessment = new RepositoryBase<Assessment>(contexto);
                }
                return repositorioAssessment;
            }
        }
        public RepositoryBase<Career> RepositorioCareer
        {
            get
            {

                if (this.repositorioCareer == null)
                {
                    this.repositorioCareer = new RepositoryBase<Career>(contexto);
                }
                return repositorioCareer;
            }
        }
        public RepositoryBase<Country> RepositorioCountry
        {
            get
            {

                if (this.repositorioCountry == null)
                {
                    this.repositorioCountry = new RepositoryBase<Country>(contexto);
                }
                return repositorioCountry;
            }
        }
        public RepositoryBase<Course> RepositorioCourse
        {
            get
            {

                if (this.repositorioCourse == null)
                {
                    this.repositorioCourse = new RepositoryBase<Course>(contexto);
                }
                return repositorioCourse;
            }
        }
        public RepositoryBase<CASU> RepositorioCASU
        {
            get
            {

                if (this.repositorioCASU == null)
                {
                    this.repositorioCASU = new RepositoryBase<CASU>(contexto);
                }
                return repositorioCASU;
            }
        }
        public RepositoryBase<Event> RepositorioEvent
        {
            get
            {

                if (this.repositorioEvent == null)
                {
                    this.repositorioEvent = new RepositoryBase<Event>(contexto);
                }
                return repositorioEvent;
            }
        }
        public RepositoryBase<Core> RepositorioCore
        {
            get
            {

                if (this.repositorioCore == null)
                {
                    this.repositorioCore = new RepositoryBase<Core>(contexto);
                }
                return repositorioCore;
            }
        }
        public RepositoryBase<Institute> RepositorioInstitute
        {
            get
            {
                if(this.repositorioInstitute == null)
                {
                    this.repositorioInstitute = new RepositoryBase<Institute>(contexto);
                }
                return repositorioInstitute;
            }
        }
        public RepositoryBase<KnowledgeArea> RepositorioKnowledgeArea
        {
            get
            {

                if (this.repositorioKnowledgeArea == null)
                {
                    this.repositorioKnowledgeArea = new RepositoryBase<KnowledgeArea>(contexto);
                }
                return repositorioKnowledgeArea;
            }
        }
        public RepositoryBase<KnowledgeSubArea> RepositorioKnowledgeSubArea
        {
            get
            {

                if (this.repositorioKnowledgeSubArea == null)
                {
                    this.repositorioKnowledgeSubArea = new RepositoryBase<KnowledgeSubArea>(contexto);
                }
                return repositorioKnowledgeSubArea;
            }
        }        
        public RepositoryBase<Notification> RepositorioNotification
        {
            get
            {

                if (this.repositorioNotification == null)
                {
                    this.repositorioNotification = new RepositoryBase<Notification>(contexto);
                }
                return repositorioNotification;
            }
        }
        public RepositoryBase<Opportunity> RepositorioOpportunity
        {
            get
            {

                if (this.repositorioOpportunity == null)
                {
                    this.repositorioOpportunity = new RepositoryBase<Opportunity>(contexto);
                }
                return repositorioOpportunity;
            }
        }
        public RepositoryBase<Period> RepositorioPeriod
        {
            get
            {

                if (this.repositorioPeriod == null)
                {
                    this.repositorioPeriod = new RepositoryBase<Period>(contexto);
                }
                return repositorioPeriod;
            }
        }
        public RepositoryBase<Profile> RepositorioProfile
        {
            get
            {

                if (this.repositorioProfile == null)
                {
                    this.repositorioProfile = new RepositoryBase<Profile>(contexto);
                }
                return repositorioProfile;
            }
        }
        public RepositoryBase<Region> RepositorioRegion
        {
            get
            {
                if (this.repositorioRegion == null)
                {
                    this.repositorioRegion = new RepositoryBase<Region>(contexto);
                }
                return repositorioRegion;
            }
        }
        public RepositoryBase<RelatedCareer> RepositorioRelatedCareer
        {
            get
            {
                if (this.repositorioRelatedCareer == null)
                {
                    this.repositorioRelatedCareer = new RepositoryBase<RelatedCareer>(contexto);
                }
                return repositorioRelatedCareer;
            }
        }
        public RepositoryBase<Representative> RepositorioRepresentative
        {
            get
            {
                if (this.repositorioRepresentative == null)
                {
                    this.repositorioRepresentative = new RepositoryBase<Representative>(contexto);
                }
                return repositorioRepresentative;
            }
        }
        public RepositoryBase<Role> RepositorioRole
        {
            get
            {

                if (this.repositorioRole == null)
                {
                    this.repositorioRole = new RepositoryBase<Role>(contexto);
                }
                return repositorioRole;
            }
        }
        public RepositoryBase<School> RepositorioSchool
        {
            get
            {

                if (this.repositorioSchool == null)
                {
                    this.repositorioSchool = new RepositoryBase<School>(contexto);
                }
                return repositorioSchool;
            }
        }
        public RepositoryBase<SchoolYear> RepositorioSchoolYear
        {
            get
            {
                if (this.repositorioSchoolYear == null)
                {
                    this.repositorioSchoolYear = new RepositoryBase<SchoolYear>(contexto);
                }
                return repositorioSchoolYear;
            }
        }
        public RepositoryBase<Score> RepositorioScore
        {
            get
            {

                if (this.repositorioScore == null)
                {
                    this.repositorioScore = new RepositoryBase<Score>(contexto);
                }
                return repositorioScore;
            }
        }
        public RepositoryBase<SentNotification> RepositorioSentNotification
        {
            get
            {
                if (this.repositorioSentNotification == null)
                {
                    this.repositorioSentNotification = new RepositoryBase<SentNotification>(contexto);
                }
                return repositorioSentNotification;
            }
        }
        public RepositoryBase<State> RepositorioState
        {
            get
            {

                if (this.repositorioState == null)
                {
                    this.repositorioState = new RepositoryBase<State>(contexto);
                }
                return repositorioState;
            }
        }    
        public RepositoryBase<Student> RepositorioStudent
        {
            get
            {

                if (this.repositorioStudent == null)
                {
                    this.repositorioStudent = new RepositoryBase<Student>(contexto);
                }
                return repositorioStudent;
            }
        }    
        public RepositoryBase<Subject> RepositorioSubject
        {
            get
            {

                if (this.repositorioSubject == null)
                {
                    this.repositorioSubject = new RepositoryBase<Subject>(contexto);
                }
                return repositorioSubject;
            }
        }
        public RepositoryBase<User> RepositorioUser
        {
            get
            {

                if (this.repositorioUser == null)
                {
                    this.repositorioUser = new RepositoryBase<User>(contexto);
                }
                return repositorioUser;
            }
        }
        #endregion

        #region Declaración de métodos
        public void Save()
        {
            try
            {
                contexto.SaveChanges();
            }
            #region Catchs
            catch (InvalidCastException e) { throw (e); }
            catch (DbUpdateException e) { throw (e); }
            catch (DbEntityValidationException e) { throw (e); }
            catch (InvalidOperationException e) { throw (e); }
            #endregion
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    contexto.Dispose();
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}