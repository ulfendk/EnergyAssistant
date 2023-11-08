using System.Collections.ObjectModel;
using UlfenDk.EnergyAssistant.Database;

namespace EnergyAssistant.ViewModels;

public class YearPeriodViewModel
{
    private YearPeriod _data;

    public YearPeriod Data => new YearPeriod
    {
        Year = _data.Year,
        Vat = _data.Vat,
        YearlyCosts = YearlyCosts.Select(x => x.Data).ToList()
    };

    public YearPeriodViewModel(YearPeriod? data)
    {
        _data = data ?? new YearPeriod();
        YearlyCosts = new ObservableCollection<FeeViewModel>(_data.YearlyCosts.Select(x => new FeeViewModel(x)));
    }

    public int Year
    {
        get => Data.Year;
        set => Data.Year = value;
    }

    public decimal Vat
    {
        get => Data.Vat;
        set => Data.Vat = value;
    }

    public ObservableCollection<FeeViewModel> YearlyCosts { get; }
}