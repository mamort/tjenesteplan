// Bør flytte dette over i en egen reducer
class Tjenesteplan {
    constructor(categories, dates) {
        this.categories = categories;
        this.dates = dates;
    }

    addDateWithCategory(startDate, categoryId) {
        const category = this.categories.find(c => c.id == categoryId);
        if(category.followedBy && category.followedBy.length > 0) {
            this.deleteDate(dates, startDate);
            this.addDate(
                this.dates,
                startDate,
                startDate,
                categoryId
            );

            for(let i = 0; i < category.followedBy.length; i++) {
                let followedByCategory = category.followedBy[i];
                const followedByDate = startDate.clone().add(i+1, 'day');
                this.deleteDate(dates, followedByDate);
                this.addDate(
                    this.dates,
                    followedByDate,
                    followedByDate,
                    followedByCategory.id
                );
            }
        }
    }

    addDate(startDate, endDate, categoryId) {

        for(let date = startDate.clone(); date.isSameOrBefore(endDate, 'day'); date.add(1, 'day')) {
            this.deleteDate(date);
        }

        for(let date = startDate.clone(); date.isSameOrBefore(endDate, 'day'); date.add(1, 'day')) {
            var index = this.dates.findIndex(d => d.date.isSame(date, 'day'));
            if(index > -1){
                this.dates.splice(index, 1)
            }

            const newDate = { date: date.clone(), categoryId: categoryId };
            this.dates.push(newDate);
        }
    }

    findMainCategoryFromSubCategory(subCategory) {
        const categories = this.categories;

        for(let i = 0; i < categories.length; i++) {
            if(categories[i].followedBy) {
                for(let k = 0; k < categories[i].followedBy.length;k++) {
                    let cat = categories[i].followedBy[k];
                    if(cat.id == subCategory) {
                        return categories[i];
                    }
                }
            }
        }

        return null;
    }

    findLastMainCategoryDate(mainCategory, date) {
        for(let i = 0; i < mainCategory.followedBy.length; i++) {
            let prevDate = date.clone().subtract(i+1, 'day');
            const indexOfDependentDate = this.dates.findIndex(d => d.date.isSame(prevDate, 'day'));
            if(indexOfDependentDate > -1) {
                const depDate = this.dates[indexOfDependentDate];
                if(depDate.categoryId == mainCategory.id) {
                    return prevDate;
                }
            }
        }

        return null;
    }

    deleteDate(date) {
        const index = this.dates.findIndex(d => d.date.isSame( date, 'day'));
        if(index > -1) {
            const savedDate = this.dates[index];
            this.dates.splice(index, 1)

            const categories = this.categories;

            // Sletter "Vakt" datoen også hvis man velger en annen kategori for "Fri etter vakt"
            const mainCategory = this.findMainCategoryFromSubCategory(savedDate.categoryId);
            if(mainCategory) {
                const mainCategoryDate = this.findLastMainCategoryDate(mainCategory, date);
                for(let i = 0; i < mainCategory.followedBy.length; i++) {
                    let nextDate = mainCategoryDate.clone().add(i, 'day');
                    const indexOfDate = this.dates.findIndex(d => d.date.isSame(nextDate, 'day'));
                    dates.splice(indexOfDate, 1);
                }
            }

            // Sletter "Fri etter vakt" datoen også hvis man velger en annen kategori for "Vakt"
            const category = categories.find(c => c.id == savedDate.categoryId);
            if(category && category.followedBy) {
                for(let i = 0; i < category.followedBy.length;i++) {
                    let nextDate = date.clone().add(i, 'day');
                    const indexOfDependentDate = this.dates.findIndex(d => d.date.isSame(nextDate, 'day'));
                    dates.splice(indexOfDependentDate, 1)
                }
           }
        }

    }

}