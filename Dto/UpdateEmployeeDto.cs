namespace HRMS_api.Dto
{
    public class UpdateEmployeeDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HiredDate { get; set; }


        public int DepartmentId { get; set; }//foreign key


        public int PositionId { get; set; }//foreign key

        public int RoleId { get; set; }//foreign key
    }
}
