function validateEmail(email) {
    var re = /\S+@\S+\.\S+/;
    if (re.test(email))
        return true
    else
        return false
}
