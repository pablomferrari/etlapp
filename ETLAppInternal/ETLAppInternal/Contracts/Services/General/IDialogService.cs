using System.Threading.Tasks;

namespace ETLAppInternal.Contracts.Services.General
{
    public interface IDialogService
    {
        Task ShowDialog(string message, string title, string buttonLabel);

        void ShowToast(string message);
        Task<bool> ShowConfirm(string message, string title, string okText, string cancelText);
    }
}
