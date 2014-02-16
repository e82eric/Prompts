
function PesterEqual($value, $expected) {
    return ($expected -ceq $value)
}

function PesterEqualFailureMessage($value, $expected) {
    return "Expected: {$expected}, But was {$value}"
}

function NotPesterEqualFailureMessage($value, $expected) {
    return "Expected: value was {$value}, but should not have been the same"
}

