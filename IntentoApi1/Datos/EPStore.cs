using IntentoApi1.Models.Dto;

namespace IntentoApi1.Datos
{
    public class EPStore
    {
        public static List<EPDto> EPList = new List<EPDto>
        {
            new EPDto{ Id = 1, Nombre = "Vista a la piscina", Ocupantes = 3, MetrosCuadrados = 50},
            new EPDto{ Id = 2, Nombre = "Vista a la playa", Ocupantes = 2, MetrosCuadrados = 100}
        };
    }
}
