using AerostarLogbook.Services.DatabaseMain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AerostarLogbook.Services.Helpers
{
    public static class ViewModelHelper
    {
        public static async Task AddModelInstance<T>(T model, ObservableCollection<T> observableList, Action<T> updateSerialNumber) where T : new()
        {
            if (model == null || observableList == null || updateSerialNumber == null)
            {
                throw new ArgumentNullException("One or more arguments are null");
            }
            // Use the database service to insert the model into the database
            await DatabaseService.InsertServiceAsync<T>(model);

            // Insert the model into the observable list
            observableList.Insert(0, model);

            // Update the model's serial number
            updateSerialNumber(model);
        }
    

        public static async Task DeleteModelInstance<T>(T model, ObservableCollection<T> observableList) where T : new()
        {
            if (model == null || observableList == null)
            {
                throw new ArgumentNullException("Model or observable list is null");
            }

            // Use the database service to delete the model from the database
            await DatabaseService.DeleteServiceAsync<T>(model);

            // Remove the model from the observable list
            observableList.Remove(model);
        }

        public static async Task UpdateModelInstance<T>(T originalModel, T updatedModel, ObservableCollection<T> observableList) where T : new()
        {
            if (originalModel == null || updatedModel == null || observableList == null)
            {
                throw new ArgumentNullException("One or more arguments are null");
            }

            // Use the database service to update the model in the database
            await DatabaseService.UpdateServiceAsync<T>(updatedModel);

            // Replace the original model in the observable list
            int index = observableList.IndexOf(originalModel);
            if (index >= 0)
            {
                observableList.Remove(originalModel);
                observableList.Insert(index, updatedModel);
            }
        }
    }
}
