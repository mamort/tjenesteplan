function getTopScrollPosition() {
    var doc = document.documentElement;
    return (window.pageYOffset || doc.scrollTop)  - (doc.clientTop || 0);

}

export default {
    getTopScrollPosition
}