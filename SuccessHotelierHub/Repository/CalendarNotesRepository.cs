using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SuccessHotelierHub.Models;

namespace SuccessHotelierHub.Repository
{
    public class CalendarNotesRepository
    {
        #region Calendar Notes

        public List<CalendarNotesVM> GetNotesById(Guid notesId)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Id", Value = notesId }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetNotesById", parameters);


            var calendarNotes = new List<CalendarNotesVM>();
            calendarNotes = DALHelper.CreateListFromTable<CalendarNotesVM>(dt);

            return calendarNotes;
        }

        public string AddCalendarNotes(CalendarNotesVM notes)
        {
            string notesId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Date", Value = notes.Date },
                    new SqlParameter { ParameterName = "@Notes", Value = notes.Notes },
                    new SqlParameter { ParameterName = "@IsActive", Value = notes.IsActive },
                    new SqlParameter { ParameterName = "@CreatedBy", Value = notes.CreatedBy }
                };

            notesId = Convert.ToString(DALHelper.ExecuteScalar("AddCalendarNotes", parameters));

            return notesId;
        }

        public string UpdateCalendarNotes(CalendarNotesVM notes)
        {
            string notesId = string.Empty;

            SqlParameter[] parameters =
                {
                    new SqlParameter { ParameterName = "@Id", Value = notes.Id },
                    new SqlParameter { ParameterName = "@Date", Value = notes.Date },
                    new SqlParameter { ParameterName = "@Notes", Value = notes.Notes },
                    new SqlParameter { ParameterName = "@IsActive", Value = notes.IsActive },
                    new SqlParameter { ParameterName = "@UpdatedBy", Value = notes.UpdatedBy }
                };

            notesId = Convert.ToString(DALHelper.ExecuteScalar("UpdateCalendarNotes", parameters));

            return notesId;
        }
      
        public List<CalendarNotesVM> GetCalendarNotesOfCurrentMonth(int month, int year)
        {
            SqlParameter[] parameters =
               {
                    new SqlParameter { ParameterName = "@Month" , Value = month },
                    new SqlParameter { ParameterName = "@Year"  , Value = year }
                };

            var dt = DALHelper.GetDataTableWithExtendedTimeOut("GetCalendarNotesOfCurrentMonth", parameters);


            var notes = new List<CalendarNotesVM>();
            notes = DALHelper.CreateListFromTable<CalendarNotesVM>(dt);

            return notes;
        }

        #endregion
    }
}