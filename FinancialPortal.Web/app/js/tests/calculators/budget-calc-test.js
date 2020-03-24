import test from 'ava';

import BudgetCalculator from '../../calculators/budget-calculator';

test('Test maths is right', t => {
    let calc = new BudgetCalculator();

    calc.addOutgoing('expenditure', 10);
    calc.addOutgoing('expenditure', 30);

    var result = calc.calculate();

    t.is(result.outgoing.expenditure, 40);
});
