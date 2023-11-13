﻿using DataAccess.CRUD;
using DataAccess.DAOs;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BookingCrudFactory : CrudFactory
    {

        public BookingCrudFactory()
        {
            _dao = SqlDao.GetInstance();
        }

        public override void Create(BaseDTO baseDTO)
        {
            var booking = baseDTO as Booking;

            var sqlOperation = new SqlOperation { ProcedureName = "CRE_BOOKING_PR" };
            sqlOperation.AddIntParam("P_idPet", booking.IdPet);
            sqlOperation.AddIntParam("P_idPayment", booking.IdPayment);
            sqlOperation.AddIntParam("P_idPackage", booking.IdPackage);
            sqlOperation.AddDateTimeParam("P_checkInDate", booking.CheckInDate);
            sqlOperation.AddDateTimeParam("P_checkOutDate", booking.CheckOutDate);
            sqlOperation.AddVarcharParam("P_considerations", booking.Considerations);
            sqlOperation.AddBoolParam("P_status", booking.Status);


            _dao.ExecuteProcedure(sqlOperation);
        }

        public override void Delete(BaseDTO baseDTO)
        {
            var booking = baseDTO as Booking;

            var sqlOperation = new SqlOperation {ProcedureName= "DELETE_BOOKING_PR" };
            sqlOperation.AddIntParam("P_ID", booking.Id);
        }

        public override T Retrieve<T>()
        {
            throw new NotImplementedException();
        }

        public override List<T> RetrieveAll<T>()
        {
            var lstBooking = new List<T>();

            var sqlOperation = new SqlOperation { ProcedureName = "RET_ALL_BOOKINGS_PR" };

            //Devuelve la lista de diccionarios
            var lstResults = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResults.Count > 0)
            {
                foreach (var row in lstResults)
                {
                    var bookingDTO = new Booking()
                    {
                        Id = (int)row["idBooking"],
                        IdPet = (int)row["idPet"],
                        IdPayment = (int)row["idPayment"],
                        IdPackage = (int)row["idPackage"],
                        CheckInDate = (DateTime)row["checkInDate"],
                        CheckOutDate = (DateTime)row["checkOutDate"],
                        Considerations = (string)row["considerations"],
                        Status = (bool)row["status"]
                    };
                    lstBooking.Add((T)Convert.ChangeType(bookingDTO, typeof(T)));
                }
            }
            return lstBooking;
        }
        private T BuildBooking<T>(Dictionary<string, object> row)
        {
            //Construir el objeto
            var booking = new Booking()
            {
                Id = (int)row["idBooking"],
                IdPet = (int)row["idPet"],
                IdPayment = (int)row["idPayment"],
                IdPackage = (int)row["idPackage"],
                CheckInDate = (DateTime)row["checkInDate"],
                CheckOutDate = (DateTime)row["checkOutDate"],
                Considerations = (string)row["considerations"],
                Status = (bool)row["status"]
            };
            return (T)Convert.ChangeType(booking, typeof(T));

        }

        public override T RetrieveById<T>(int id)
        {
            var sqlOperation = new SqlOperation { ProcedureName = "RET_ID_BOOKING_PR" };

            sqlOperation.AddIntParam("ID", id);

            var lstResults = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResults.Count > 0)
            {
                var row = lstResults[0];

                //Construir el objeto
                var bookingDTO = BuildBooking<T>(row);
                return bookingDTO;

            }
            return default(T);
        }

        public override void Update(BaseDTO baseDTO)
        {
            throw new NotImplementedException();
        }
    }
}