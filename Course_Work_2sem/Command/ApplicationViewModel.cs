using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using System.Windows;

namespace Course_Work_2sem
{
    class ApplicationsViewModel
    {
        //Добавление всех сериалов
        private RelayCommand allSeries;
        public RelayCommand AllSeries
        {
            get
            {

                return allSeries ?? (allSeries = new RelayCommand(obj =>
                 {
                    SeriesUser.SeriesU.AllSeries();
                 }));
            }
        }

        //Список сериалов Юзера
        private RelayCommand mySeries;
        public RelayCommand MySeries
        {
            get
            {

                return mySeries ?? (mySeries= new RelayCommand(obj =>
                {
                    SeriesUser.SeriesU.MySeries();
                }));
            }
        }
        //Аккаунт Юзера
        private RelayCommand myProfile;
        public RelayCommand MyProfile
        {
            get
            {

                return myProfile ?? (myProfile= new RelayCommand(obj =>
                {
                    if (MyAccount.isOpened == false)
                    {
                        MyAccount.isOpened = true;
                        SeriesUser.SeriesU.MyAccount();
                    }
                }));
            }
        }

        //Список премьер
        private RelayCommand premier;
        public RelayCommand Premier
        {
            get
            {

                return premier ?? (premier = new RelayCommand(obj =>
                {
                    SeriesUser.SeriesU.Premier();
                }));
            }
        }

        //Выход
        private RelayCommand exit;
        public RelayCommand Exit
        {
            get
            {

                return exit ?? (exit = new RelayCommand(obj =>
                {
                    SeriesUser.SeriesU.ExitEnter();
                }));
            }
        }



        //панель админа
        private RelayCommand adminPanel;
        public RelayCommand AdminPanel
        {
            get
            {

                return adminPanel ?? (adminPanel= new RelayCommand(obj =>
                {
                    if (AdminPanel.isOpened == false)
                    {
                        AdminPanel.isOpened = true;
                        SeriesUser.SeriesU.ControlItems();
                    }
                }));
            }
        }

        //Поиск по категориям
        private RelayCommand searchBy;
        public RelayCommand SearchBy
        {
            get
            {

                return searchBy ?? (searchBy = new RelayCommand(obj =>
                {
                    SeriesUser.SeriesU.SearchForCategory();
                }));
            }
        }
    }
}

