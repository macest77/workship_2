using System;
using System.Collections.Generic;
using System.Text;

namespace apteka
{
    public abstract class AciveRecord
    {
        public abstract int ID { get; }

        /// <summary>
        /// Metoda zapisująca obiekt do bazy danych.
        /// </summary>
        public abstract void Save();
        /// <summary>
        /// Metoda wczytująca dane do obiektu.
        /// </summary>
        public abstract void Reload();
        /// <summary>
        /// Metoda chroniona pozwalająca otworzyć połączenie z bazą danych.
        /// </summary>
        protected abstract void Open();
        /// <summary>
        /// Metodę chronioną pozwalającą zamknąć połączenie z bazą danych.
        /// </summary>
        protected abstract void Close();
        /// <summary>
        /// Metoda abstrakcyjna usuwająca obiekt bazy danych.
        /// </summary>
        public abstract void Remove();
    }
}
