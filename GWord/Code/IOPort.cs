using System;
using System.Collections;
using System.Collections.Generic;

namespace GuessEngine
{
    /// <summary>
    /// Обёртка для интерфейсов. Класс хранит ссылку на объект, через который он получает оступ к интерфейсу. Используется главным классом программы (GuessDriver)
    ///   для вывода информации в интерфейс
    /// </summary>
    public abstract class IOPort
    {
        protected object _interfaceLink = null;

        public IOPort(object InterfaceLink)
        {
            this._interfaceLink = InterfaceLink;
        }

        public abstract void printFindedData(IEnumerable<string>ToPrint);
    }
}