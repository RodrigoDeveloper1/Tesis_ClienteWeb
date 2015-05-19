using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesis_ClienteWeb.Models;
using Tesis_ClienteWeb_Data.Services;
using Tesis_ClienteWeb_Models.POCO;
using ClosedXML.Excel;
using Tesis_ClienteWeb_Data.Repositories;

namespace Tesis_ClienteWeb.Controllers
{
    public class AlumnosController : MaestraController
    {
        private string _controlador = "Alumnos";
        private BridgeController _puente = new BridgeController();

        [HttpGet]
        public ActionResult AgregarAlumno()
        {
            ConfiguracionInicial(_controlador, "AgregarAlumno");
            #region Inicialización de variables
            AgregarAlumnosModel model = new AgregarAlumnosModel();
            SchoolService _schoolService = new SchoolService();
            #endregion
            #region Sección mensajes TempData
            if (TempData["AlumnoAgregado"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["AlumnoAgregado"].ToString();
            }
            else if (TempData["CatchError"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["CatchError"].ToString());
            }
            else if (TempData["CursoNoSeleccionado"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["CursoNoSeleccionado"].ToString());
            }
            else if (TempData["AlumnosNoAgregados"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["AlumnosNoAgregados"].ToString());
            }
            else if (TempData["ErrorMatricula"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorMatricula"].ToString());
            }
            else if (TempData["ErrorFirstLastName"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorFirstLastName"].ToString());
            }
            else if (TempData["ErrorSecondLastName"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorSecondLastName"].ToString());
            }
            else if (TempData["ErrorFirstName"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["ErrorSecondLastName"].ToString());
            }
            #endregion

            return View(model);
        }

        [HttpPost]
        public bool AgregarAlumno(List<Object> values)
        {
            //Si entra en [HttpGet] AgregarAlumno(), no necesita de ConfiguracionInicial();
            #region Declaración de variables
            int idCurso = 0;
            CourseService _courseService = new CourseService();
            Course curso;
            int numberList = 0;
            int registrationNumber = 0;
            string firstLastName = "";
            string secondLastName = "";
            string firstName = "";
            string secondName = "";
            #endregion

            #region Validación de id del curso
            //Dentro de la lista de valores, el primer valor debe ser el id del curso.
            if (values == null || values[0] == null || values[0].ToString().Equals(""))
            {
                TempData["CursoNoSeleccionado"] = "Por favor seleccione un curso.";
                return false;
            }
            else
                idCurso = Convert.ToInt32(values[0]);
            #endregion
            #region Validación de alumnos añadidos
            if (values.Count == 1)
            {
                TempData["AlumnosNoAgregados"] = "Por favor inserte al menos un alumno.";
                return false;
            }
            #endregion
            #region Obteniendo el curso
            curso = _courseService.ObtenerCursoPor_Id(idCurso);
            #endregion
            #region Ciclo de asignación de la lista de alumnos
            for (int i = 1; i <= values.Count - 1; i++)
            {
                string[] valores = values[i].ToString().Split(',');

                #region Asignación de numberList
                numberList = Convert.ToInt32(valores[0]);
                #endregion
                #region Validación de registrationNumber
                try
                {
                    registrationNumber = Convert.ToInt32(valores[1]);
                }
                catch (FormatException e)
                {
                    TempData["ErrorMatricula"] = "El alumno nro. '" + numberList + "' no posee un formato de " +
                        "matrícula correcto, por favor corregir para poder agregar la lista de estudiantes." +
                        " El error es: " + e.Message;

                    return false;
                }
                catch (Exception e)
                {
                    throw e;
                }
                #endregion
                #region Validación de firstLastName
                firstLastName = valores[2].ToString();
                if (firstLastName.Equals(""))
                {
                    TempData["ErrorFirstLastName"] = "El alumno nro. '" + numberList + "' no posee primer" +
                        " apellido, por favor corregir para poder agregar la lista de estudiantes.";
                    return false;
                }
                #endregion
                #region Validación de secondLastName
                secondLastName = valores[3].ToString();
                if (secondLastName.Equals(""))
                {
                    TempData["ErrorSecondLastName"] = "El alumno nro. '" + numberList + "' no posee segundo" +
                        " apellido, por favor corregir para poder agregar la lista de estudiantes.";
                    return false;
                }
                #endregion
                #region Validación de firstName
                firstName = valores[4].ToString();
                if (firstName.Equals(""))
                {
                    TempData["ErrorFirstName"] = "El alumno nro. '" + numberList + "' no posee primer" +
                        " nombre, por favor corregir para poder agregar la lista de estudiantes.";
                    return false;
                }
                #endregion
                #region Asignación de secondName
                secondName = valores[5].ToString();
                #endregion
                #region Creación de nuevo estudiante
                Student estudiante = new Student()
                {
                    NumberList = numberList,
                    RegistrationNumber = registrationNumber,
                    FirstLastName = firstLastName,
                    SecondLastName = secondLastName,
                    FirstName = firstName,
                    SecondName = secondName
                };
                #endregion
                #region Asignación de estudiante al curso
                curso.Students.Add(estudiante);
                #endregion
            }
            #endregion
            #region Modificando el curso
            try
            {
                _courseService.ModificarCourse(curso);
                TempData["AlumnoAgregado"] = "Los alumnos fueron agregados correctamente en el curso '" +
                curso.Name + "'";

                return true;
            }
            #endregion
            #region Catch del error
            catch (Exception e)
            {
                TempData["CatchError"] = e.Message;
                return false;
            }
            #endregion
        }

        [HttpGet]
        public ActionResult GestionAlumnos()
        {
            ConfiguracionInicial(_controlador, "GestionAlumnos");
            AgregarAlumnosModel model = new AgregarAlumnosModel();

            if (TempData["Exception"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Exception"].ToString());

            }
            else if (TempData["Eliminado"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["Eliminado"].ToString();
            }
            else if (TempData["Ok"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["Ok"].ToString();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult EliminarAlumno(int id)
        {
            ConfiguracionInicial(_controlador, "EliminarAlumno");
            List<object> jsonResult = new List<object>();
            StudentService studentService = new StudentService();
            Student student = studentService.ObtenerAlumnoPorId(id);

            try
            {
                studentService.EliminarStudent(id);
                TempData["Eliminado"] = "El alumno '" + student.FirstName + " " + student.FirstLastName +
                    "' fue eliminado.";
                jsonResult.Add(new { success = true });
            }
            catch (Exception e)
            {
                TempData["Exception"] = e.ToString();
                jsonResult.Add(new { success = false });
            }

            return Json(jsonResult);
        }

        [HttpGet]
        public ActionResult EditarAlumno(int id)
        {
            ConfiguracionInicial(_controlador, "EditarAlumno");
            EditarAlumnoModel model = new EditarAlumnoModel(id);

            #region Sección TempData
            if (TempData["ModelStateInvalid"] != null)
            {
                model.MostrarErrores = "block";

                for (int contador = 1; contador <= (int)TempData["ContadorModelStateErrors"]; contador++)
                {
                    if (TempData["Error_" + contador].ToString() != "")
                        ModelState.AddModelError("", TempData["Error_" + contador].ToString());
                }
            }
            else if (TempData["Exception"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Exception"].ToString());
            }
            else if (TempData["Ok"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["Ok"].ToString();
            }
            #endregion

            #region Inicializando representantes
            if (model.Student.Representatives.Count == 0)
            {
                model.Student.Representatives.Add(new Representative());
                model.Student.Representatives.Add(new Representative());
            }
            else if (model.Student.Representatives.Count == 1)
            {
                model.Student.Representatives.Add(new Representative());
            }
            #endregion
            #region Inicializando tipos de cédula & tipos de sexo
            model.selectListTiposCedula = _puente.InicializadorListaTiposCedula(model.selectListTiposCedula);
            model.selectListSexos = _puente.InicializadorListaSexos(model.selectListSexos);
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult EditarAlumno(EditarAlumnoModel model)
        {
            //Si entra en [HttpGet] EditarAlumno(int id), no necesita de ConfiguracionInicial();
            #region Declaración de variables
            Student student = new Student();
            StudentService studentService = new StudentService();
            Representative representante1 = new Representative();
            Representative representante2 = new Representative();
            bool incluirRepresentante1 = true;
            bool incluirRepresentante2 = true;
            #endregion

            #region Validando el modelo
            if (!ModelState.IsValid)
            {
                if (model.Student.FirstName == null || model.Student.SecondName == null ||
                    model.Student.FirstLastName == null || model.Student.SecondLastName == null)
                {
                    TempData["ModelStateInvalid"] = true;
                    TempData["ContadorModelStateErrors"] = ModelState.Count;
                    int contador = 1;

                    foreach (ModelState error in ModelState.Values)
                    {
                        TempData["Error_" + contador] = (error.Errors.Count != 0 ?
                            error.Errors[0].ErrorMessage : "");
                        contador++;
                    }

                    return RedirectToAction("EditarAlumno");
                }
            }
            #endregion
            #region Obteniendo el estudiante & actualizando datos
            student = studentService.ObtenerAlumnoPorId(model.Student.StudentId);
            student.NumberList = model.Student.NumberList;
            student.FirstLastName = model.Student.FirstLastName;
            student.FirstName = model.Student.FirstName;
            student.RegistrationNumber = model.Student.RegistrationNumber;
            student.SecondLastName = model.Student.SecondLastName;
            student.SecondName = model.Student.SecondName;
            #endregion
            #region Representante #1
            try
            {
                representante1.Name = model.Student.Representatives[0].Name;
                representante1.Email = model.Student.Representatives[0].Email;
                representante1.Gender = model.Student.Representatives[0].Gender;
                representante1.IdentityNumber = model.Student.Representatives[0].IdentityNumber;
                representante1.LastName = model.Student.Representatives[0].LastName;
                representante1.SecondLastName = model.Student.Representatives[0].SecondLastName;
                if (representante1.Name == null || representante1.Email == null ||
                    representante1.IdentityNumber == null || representante1.LastName == null ||
                    representante1.SecondLastName == null)
                {
                    incluirRepresentante1 = false;
                }
                else
                    if(student.Representatives.Count != 0)
                        student.Representatives[0] = representante1;
                    else
                        student.Representatives.Add(representante1);
            }
            catch(Exception)
            {
                incluirRepresentante1 = false;
            }
            #endregion
            #region Representante #2
            try
            {
                representante2.Name = model.Student.Representatives[1].Name;
                representante2.Email = model.Student.Representatives[1].Email;
                representante2.Gender = model.Student.Representatives[1].Gender;
                representante2.IdentityNumber = model.Student.Representatives[1].IdentityNumber;
                representante2.LastName = model.Student.Representatives[1].LastName;
                representante2.SecondLastName = model.Student.Representatives[1].SecondLastName;
                if (representante2.Name == null || representante2.Email == null ||
                    representante2.IdentityNumber == null || representante2.LastName == null ||
                    representante2.SecondLastName == null)
                {
                    incluirRepresentante2 = false;
                }
                else
                    if (student.Representatives.Count == 0 || student.Representatives.Count == 1)
                        student.Representatives.Add(representante2);
                    else
                        student.Representatives[1] = representante2;
            }
            catch (Exception)
            {
                incluirRepresentante2 = false;
            }
            #endregion
            #region Modificando el estudiante
            try
            {
                studentService.ModificarEstudiante(student);
            }
            catch (Exception e)
            {
                TempData["Exception"] = e.ToString();
            }
            #endregion

            #region Acción exitosa
            if (incluirRepresentante1 && incluirRepresentante2)
                TempData["Ok"] = "Se ha modificado correctamente el alumno: " + model.Student.SecondLastName +
                    " " + model.Student.FirstLastName + ", " + model.Student.FirstName + ".";
            else if (incluirRepresentante1 || incluirRepresentante2)
                TempData["Ok"] = "Se ha modificado correctamente el alumno: " + model.Student.SecondLastName +
                    " " + model.Student.FirstLastName + ", " + model.Student.FirstName + ". Solo se ha " + 
                    "incluido un representante.";
            else
                TempData["Ok"] = "Se ha modificado correctamente el alumno: " + model.Student.SecondLastName +
                    " " + model.Student.FirstLastName + ", " + model.Student.FirstName + ". No se " +
                    "incluyeron los representantes en esta edición";
            #endregion

            return RedirectToAction("GestionAlumnos");
        }

        [HttpGet]
        public ActionResult AsociarRepresentantes()
        {
            ConfiguracionInicial(_controlador, "AsociarRepresentantes");
            AsociarRepresentantesModel model = new AsociarRepresentantesModel();

            #region Sección TempData
            if (TempData["Exception"] != null)
            {
                model.MostrarErrores = "block";
                ModelState.AddModelError("", TempData["Exception"].ToString());
            }
            else if (TempData["Modificado"] != null)
            {
                model.MostrarAclamaciones = "block";
                model.MensajeAclamacion = TempData["Modificado"].ToString();
            }
            else if (TempData["ModelStateInvalid"] != null)
            {
                model.MostrarErrores = "block";

                for (int contador = 1; contador <= (int)TempData["ContadorModelStateErrors"]; contador++)
                {
                    if (TempData["Error_" + contador].ToString() != "")
                        ModelState.AddModelError("", TempData["Error_" + contador].ToString());
                }
            }
            #endregion

            model.selectListTiposCedula = _puente.InicializadorListaTiposCedula(model.selectListTiposCedula);
            model.selectListSexos = _puente.InicializadorListaSexos(model.selectListSexos);

            return View(model);
        }

        [HttpPost]
        public JsonResult AsociarRepresentantes(int idEstudiante,
            string poseeRepresentante_1, int representante1_id, string representante1_cedula,
            string representante1_sexo, string representante1_nombre, string representante1_apellido1,
            string representante1_apellido2, string representante1_correo,
            string poseeRepresentante_2, int representante2_id, string representante2_cedula,
            string representante2_sexo, string representante2_nombre, string representante2_apellido1,
            string representante2_apellido2, string representante2_correo)
        {
            //Si entra en [HttpGet] AsociarRepresentantes(), no necesita de ConfiguracionInicial();
            #region Declaración de variables
            List<object> jsonResult = new List<object>();
            Representative representante1 = new Representative();
            Representative representante2 = new Representative();
            Student student = new Student();
            bool agregarRepresentante1 = true;
            bool agregarRepresentante2 = true;
            int contador = 0; //Contador de errores. Solo para la validación del modelo.
            int i = 1; //Contador de los mensajes de errores. Solo para la validación del modelo.

            UnitOfWork unidad = new UnitOfWork();
            StudentService studentService = new StudentService(unidad);
            RepresentativeService representativeService = new RepresentativeService(unidad);
            #endregion

            #region Validación del modelo - Representante #1
            if (representante1_cedula.Equals("V-") || representante1_cedula.Equals("E-") ||
                representante1_nombre.Equals("") || representante1_apellido1.Equals("") ||
                representante1_apellido2.Equals(""))
            {
                #region Datos inválidos
                #region Cédula
                if (representante1_cedula.Equals("V-") || representante1_cedula.Equals("E-"))
                {
                    ModelState.AddModelError("", "Por favor ingrese la cédula del representante #1");
                    contador++;
                }
                #endregion
                #region Nombre
                if (representante1_nombre.Equals(""))
                {
                    ModelState.AddModelError("", "Por favor ingrese el nombre del representante #1");
                    contador++;
                }
                #endregion
                #region Apellido 1
                if (representante1_apellido1.Equals(""))
                {
                    ModelState.AddModelError("", "Por favor ingrese el apellido del representante #1");
                    contador++;
                }
                #endregion
                #region Apellido 2
                if (representante1_apellido2.Equals(""))
                {
                    ModelState.AddModelError("", "Por favor ingrese el segundo apellido del representante #1");
                    contador++;
                }
                #endregion
                #endregion
                #region Especificando no agregar - Representante #1
                agregarRepresentante1 = false;
                #endregion
                #region Inicializando TempDatas respectivos
                TempData["ModelStateInvalid"] = true;
                TempData["ContadorModelStateErrors"] = contador;

                foreach (ModelState error in ModelState.Values)
                {
                    TempData["Error_" + i] = (error.Errors.Count != 0 ? error.Errors[0].ErrorMessage : "");
                    i++;
                }
                #endregion
            }
            #endregion
            #region Validación del modelo - Representante #2
            if (representante2_cedula.Equals("V-") || representante2_cedula.Equals("E-") ||
                representante2_nombre.Equals("") || representante2_apellido1.Equals("") ||
                representante2_apellido2.Equals(""))
            {
                #region Datos inválidos
                #region Cédula
                if (representante2_cedula.Equals("V-") || representante2_cedula.Equals("E-"))
                {
                    ModelState.AddModelError("", "Por favor ingrese la cédula del representante #2");
                    contador++;
                }
                #endregion
                #region Nombre
                if (representante2_nombre.Equals(""))
                {
                    ModelState.AddModelError("", "Por favor ingrese el nombre del representante #2");
                    contador++;
                }
                #endregion
                #region Apellido 1
                if (representante2_apellido1.Equals(""))
                {
                    ModelState.AddModelError("", "Por favor ingrese el apellido del representante #2");
                    contador++;
                }
                #endregion
                #region Apellido 2
                if (representante2_apellido2.Equals(""))
                {
                    ModelState.AddModelError("", "Por favor ingrese el segundo apellido del representante #2");
                    contador++;
                }
                #endregion
                #endregion
                #region Especificando no agregar - Representante #2
                agregarRepresentante2 = false;
                #endregion
                #region Inicializando TempDatas respectivos
                TempData["ModelStateInvalid"] = true;
                TempData["ContadorModelStateErrors"] = contador;

                foreach (ModelState error in ModelState.Values)
                {
                    TempData["Error_" + i] = (error.Errors.Count != 0 ? error.Errors[0].ErrorMessage : "");
                    i++;
                }
                #endregion
            }
            #endregion

            #region Obteniendo el estudiante
            student = studentService.ObtenerAlumnoPorId(idEstudiante);
            #endregion

            #region No posee representante #1
            if (poseeRepresentante_1 == null) //No posee representante
            {
                representante1.Email = representante1_correo;
                representante1.IdentityNumber = representante1_cedula;
                representante1.Gender = (representante1_sexo.Equals("Femenino") ? false : true);
                representante1.LastName = representante1_apellido1;
                representante1.Name = representante1_nombre;
                representante1.SecondLastName = representante1_apellido2;
                representante1.Students.Add(student);
            }
            #endregion
            #region Si posee representante #1
            else
            {
                representante1 = representativeService.ObtenerRepresentantePorId(representante1_id);
                representante1.Email = representante1_correo;
                representante1.IdentityNumber = representante1_cedula;
                representante1.Gender = (representante1_sexo.Equals("Femenino") ? false : true);
                representante1.LastName = representante1_apellido1;
                representante1.Name = representante1_nombre;
            }
            #endregion

            #region No posee representante #2
            if (poseeRepresentante_2 == null) //No posee representante
            {
                representante2.Email = representante2_correo;
                representante2.IdentityNumber = representante2_cedula;
                representante2.Gender = (representante2_sexo.Equals("Femenino") ? false : true);
                representante2.LastName = representante2_apellido1;
                representante2.Name = representante2_nombre;
                representante2.SecondLastName = representante2_apellido2;
                representante2.Students.Add(student);
            }
            #endregion
            #region Si posee representante #2
            else
            {
                representante2 = representativeService.ObtenerRepresentantePorId(representante2_id);
                representante2.Email = representante2_correo;
                representante2.IdentityNumber = representante2_cedula;
                representante2.Gender = (representante2_sexo.Equals("Femenino") ? false : true);
                representante2.LastName = representante2_apellido1;
                representante2.Name = representante2_nombre;
            }
            #endregion

            #region Guardando representantes
            try
            {
                if (agregarRepresentante1 && poseeRepresentante_1 == null)
                    representativeService.GuardarRepresentante(representante1);
                else if (agregarRepresentante1 && poseeRepresentante_1 != null)
                    representativeService.ModificarRepresentante(representante1);

                if (poseeRepresentante_2 == null && agregarRepresentante2)
                    representativeService.GuardarRepresentante(representante2);
                else if (poseeRepresentante_2 != null && agregarRepresentante2)
                    representativeService.ModificarRepresentante(representante2);

                if (agregarRepresentante1 || agregarRepresentante2)
                    TempData["Modificado"] = "Se han actualizado los datos de los representantes del alumno '" +
                    student.FirstName + " " + student.FirstLastName + "'.";

                jsonResult.Add(new { success = true });
            }
            #endregion
            #region Cacth del error
            catch (Exception e)
            {
                TempData["Exception"] = e.ToString();
                jsonResult.Add(new { success = false });
            }
            #endregion

            return Json(jsonResult);
        }

        #region Pantalla Alumnos Principal

       
        public ActionResult Alumnos()
        {
            ObteniendoSesion();
            #region Inicializando variables
            StudentService _studentService = new StudentService();
            CourseService _courseService = new CourseService();
            
            AlumnosModel model = new AlumnosModel();
            List<Course> listaCursos;
            #endregion
            #region Inicializando SelectList de Cursos
            string idsession = (string)Session["UserId"];
            listaCursos = _courseService.ObtenerListaCursosPor_Docente(idsession, _session.SCHOOLYEARID).ToList<Course>();
            listaCursos = (listaCursos.Count == 0) ? new List<Course>() : listaCursos;
            model.selectListCursos = new SelectList(listaCursos, "CourseId", "Name");
            //model.listaAlumnos = _studentService.ObtenerListaAlumnos().ToList();
            #endregion

            return View(model);
        }

        #endregion
    }
}