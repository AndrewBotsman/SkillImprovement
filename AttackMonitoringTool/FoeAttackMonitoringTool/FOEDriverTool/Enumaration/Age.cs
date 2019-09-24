using FOEDriverTool.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOEDriverTool.Enumaration
{
    public enum Age
    {
        [StringValue("IA", "Железный Век")]
        IA = 1,
        [StringValue("EMA", "Раннее Средневековье")]
        EMA,
        [StringValue("HMA", "Высокое Средневековье")]
        HMA,
        [StringValue("CA", "Колониальный Период")]
        CA,
        [StringValue("IndA", "Индустриальная Эпоха")]
        IndA,
        [StringValue("PE", "Эпоха Прогрессивизма")]
        PE,
        [StringValue("ME", "Эпоха Модерна")]
        ME,
        [StringValue("PME", "Эпоха Постмодерна")]
        PME,
        [StringValue("CE", "Новейшее Время")]
        CE,
        [StringValue("T", "Завтра")]
        T,
        [StringValue("F", "Будущее")]
        F,
        [StringValue("AA", "Все Эпохи (Медальная)")]
        AA
    }
}
