using Studiji_za_animaciju.Data;
using Studiji_za_animaciju.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Studiji_za_animaciju.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AnimationContext _ctx = new AnimationContext();

        public ObservableCollection<Studio> Studios { get; set; } = new ObservableCollection<Studio>();
        public ObservableCollection<Show> Shows { get; set; } = new ObservableCollection<Show>();

        private Studio _selectedStudio;
        public Studio SelectedStudio
        {
            get => _selectedStudio;
            set
            {
                _selectedStudio = value;
                OnPropertyChanged("SelectedStudio");
                LoadShowsForSelectedStudio();
            }
        }

        private Show _selectedShow;
        public Show SelectedShow
        {
            get => _selectedShow;
            set { _selectedShow = value; OnPropertyChanged("SelectedShow"); }
        }

        public RelayCommand AddStudioCommand { get; }
        public RelayCommand EditStudioCommand { get; }
        public RelayCommand DeleteStudioCommand { get; }

        public RelayCommand AddShowCommand { get; }
        public RelayCommand EditShowCommand { get; }
        public RelayCommand DeleteShowCommand { get; }

        public MainViewModel()
        {
            AddStudioCommand = new RelayCommand(_ => AddStudio());
            EditStudioCommand = new RelayCommand(_ => EditStudio(), _ => SelectedStudio != null);
            DeleteStudioCommand = new RelayCommand(_ => DeleteStudio(), _ => SelectedStudio != null);

            AddShowCommand = new RelayCommand(_ => AddShow(), _ => SelectedStudio != null);
            EditShowCommand = new RelayCommand(_ => EditShow(), _ => SelectedShow != null);
            DeleteShowCommand = new RelayCommand(_ => DeleteShow(), _ => SelectedShow != null);

            LoadData();
        }

        private void LoadData()
        {
            Studios.Clear();
            foreach (var s in _ctx.Studios.ToList())
                Studios.Add(s);

            if (Studios.Any())
                SelectedStudio = Studios.First();
        }

        private void LoadShowsForSelectedStudio()
        {
            Shows.Clear();
            if (SelectedStudio == null) return;
            var shows = _ctx.Shows.Where(x => x.StudioId == SelectedStudio.StudioId).ToList();
            foreach (var sh in shows) Shows.Add(sh);
        }

        private void AddStudio()
        {
            var win = new Views.StudioWindow(new Studio());
            if (win.ShowDialog() == true)
            {
                var studio = win.Studio;
                _ctx.Studios.Add(studio);
                _ctx.SaveChanges();
                Studios.Add(studio);
                SelectedStudio = studio;
            }
        }

        private void EditStudio()
        {
            var copy = new Studio
            {
                StudioId = SelectedStudio.StudioId,
                Name = SelectedStudio.Name,
                Country = SelectedStudio.Country
            };
            var win = new Views.StudioWindow(copy);
            if (win.ShowDialog() == true)
            {
                var studio = win.Studio;
                var dbStudio = _ctx.Studios.FirstOrDefault(x => x.StudioId == studio.StudioId);
                if (dbStudio != null)
                {
                    dbStudio.Name = studio.Name;
                    dbStudio.Country = studio.Country;
                    _ctx.SaveChanges();
                    LoadData();
                }
            }
        }

        private void DeleteStudio()
        {
            if (MessageBox.Show("Delete selected studio and its shows?", "Confirm", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            var dbStudio = _ctx.Studios.FirstOrDefault(x => x.StudioId == SelectedStudio.StudioId);
            if (dbStudio != null)
            {
                // remove related shows
                var shows = _ctx.Shows.Where(x => x.StudioId == dbStudio.StudioId).ToList();
                foreach (var s in shows) _ctx.Shows.Remove(s);
                _ctx.Studios.Remove(dbStudio);
                _ctx.SaveChanges();
                Studios.Remove(SelectedStudio);
                SelectedStudio = Studios.FirstOrDefault();
            }
        }

        private void AddShow()
        {
            var show = new Show { StudioId = SelectedStudio.StudioId };
            var win = new Views.ShowWindow(show);
            if (win.ShowDialog() == true)
            {
                var s = win.ShowModel;
                _ctx.Shows.Add(s);
                _ctx.SaveChanges();
                Shows.Add(s);
            }
        }

        private void EditShow()
        {
            var copy = new Show
            {
                ShowId = SelectedShow.ShowId,
                Title = SelectedShow.Title,
                Genre = SelectedShow.Genre,
                Year = SelectedShow.Year,
                StudioId = SelectedShow.StudioId
            };
            var win = new Views.ShowWindow(copy);
            if (win.ShowDialog() == true)
            {
                var s = win.ShowModel;
                var db = _ctx.Shows.FirstOrDefault(x => x.ShowId == s.ShowId);
                if (db != null)
                {
                    db.Title = s.Title;
                    db.Genre = s.Genre;
                    db.Year = s.Year;
                    _ctx.SaveChanges();
                    LoadShowsForSelectedStudio();
                }
            }
        }

        private void DeleteShow()
        {
            if (MessageBox.Show("Delete selected show?", "Confirm", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            var db = _ctx.Shows.FirstOrDefault(x => x.ShowId == SelectedShow.ShowId);
            if (db != null)
            {
                _ctx.Shows.Remove(db);
                _ctx.SaveChanges();
                Shows.Remove(SelectedShow);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
