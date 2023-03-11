namespace MegaSenaWeb.Interfaces
{
    public interface IGeraNumeroMegaSena
    {
        Task<IEnumerable<int>> GetNumeroMegaSena();
    }
}
