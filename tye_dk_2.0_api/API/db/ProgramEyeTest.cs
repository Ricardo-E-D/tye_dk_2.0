using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tye.db;

namespace tye.API
{
    public partial class API
    {

        private Data.ProgramEyeTest programeyetest_get_single(ProgramEyeTest data)
        {
            var element = (Data.ProgramEyeTest)OW.WrapObject(typeof(Data.ProgramEyeTest), data);
            return element;
        }

        public void ProgramEyeTestDeletePermanently(int ID)
        {
            using (DatabaseEntities dc = new DatabaseEntities(connectionString))
            {
                var element = (from n in dc.ProgramEyeTests
                               where n.ID == ID
                               select n).FirstOrDefault();

                if (element != null)
                {
                    dc.ProgramEyeTests.DeleteObject(element);
                    dc.SaveChanges();
                }
            }
        }

        public void ProgramEyeTestDeletePermanently(int ProgramID, int EyeTestID)
        {
            using (DatabaseEntities dc = new DatabaseEntities(connectionString))
            {
                var element = (from n in dc.ProgramEyeTests
                               where n.ProgramID == ProgramID && n.EyeTestID == EyeTestID
                               select n).FirstOrDefault();

                if (element != null)
                {
                    dc.ProgramEyeTests.DeleteObject(element);
                    dc.SaveChanges();
                }
            }
        }

        public Data.ProgramEyeTest ProgramEyeTestGetSingle(int ID)
        {
            Data.ProgramEyeTest data = null;
            using (DatabaseEntities dc = new DatabaseEntities(connectionString))
            {
                var element = (from n in dc.ProgramEyeTests
                               where n.ID == ID
                               select n).FirstOrDefault();

                if (element != null)
                {
                    data = programeyetest_get_single(element);
                }
            }
            return data;
        }

        public List<Data.ProgramEyeTest> ProgramGetCollection(int ProgramID)
        {
            List<Data.ProgramEyeTest> data = new List<Data.ProgramEyeTest>();
            using (DatabaseEntities dc = new DatabaseEntities(connectionString))
            {
                var elements = (from n in dc.ProgramEyeTests
                                where n.ProgramID == ProgramID
                                select n);

                foreach (var element in elements)
                    data.Add(programeyetest_get_single(element));
            }
            return data;
        }

        public void ProgramEyeTestSave(Data.ProgramEyeTest savedata, bool PreventDoubles = true)
        {
            using (DatabaseEntities dc = new DatabaseEntities(connectionString))
            {
                ProgramEyeTest existing = null;

                if (savedata.ID == 0 && PreventDoubles) {  
                    existing = (from n in dc.ProgramEyeTests
                        where n.ProgramID == savedata.ProgramID && n.EyeTestID == savedata.EyeTestID
                        select n)
                        .FirstOrDefault();
                }

                if (existing == null)
                {
                    ProgramEyeTest entry = new ProgramEyeTest();
                    entry.ID = 0;
                    entry.Locked = savedata.Locked;
                    entry.LockedByOptician = savedata.LockedByOptician;
                    entry.ProgramID = savedata.ProgramID;
                    entry.EyeTestID = savedata.EyeTestID;
                    entry.Priority = savedata.Priority;
                    entry.Active = savedata.Active;
                    dc.ProgramEyeTests.AddObject(entry);
                    dc.SaveChanges();
                }
                else
                {
                    existing.Active = savedata.Active;
                    existing.Locked = savedata.Locked;
                    existing.LockedByOptician = savedata.LockedByOptician;
                    existing.Priority = savedata.Priority;
                    dc.SaveChanges();
                }
            }
        }

        public void ProgramEyeTestSave(int ProgramID, int EyeTestID, bool Locked, int Priority, bool LockedByOptician = false)
        {
            using (DatabaseEntities dc = new DatabaseEntities(connectionString))
            {
                var existing = (from n in dc.ProgramEyeTests
                                where n.ProgramID == ProgramID && n.EyeTestID == EyeTestID
                                select n).FirstOrDefault();
                if (existing == null)
                {
                    ProgramEyeTest entry = new ProgramEyeTest();
                    entry.ID = 0;
                    entry.Locked = Locked;
                    entry.LockedByOptician = LockedByOptician;
                    entry.ProgramID = ProgramID;
                    entry.EyeTestID = EyeTestID;
                    entry.Priority = Priority;
                    dc.ProgramEyeTests.AddObject(entry);
                    dc.SaveChanges();
                }
                else
                {
                    existing.Locked = Locked;
                    existing.LockedByOptician = LockedByOptician;
                    existing.Priority = Priority;
                    dc.SaveChanges();
                }
            }
        }

        public void ProgramEyeTestUpdateActive(int ID, bool Active)
        {
            using (DatabaseEntities dc = new DatabaseEntities(connectionString))
            {
                var existing = (from n in dc.ProgramEyeTests
                                where n.ID == ID
                                select n).FirstOrDefault();
                
                if(existing != null) 
                {
                    existing.Active = Active;
                    dc.SaveChanges();
                }
            }
        }

    }
}