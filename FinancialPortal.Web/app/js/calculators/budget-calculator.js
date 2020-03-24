export default class Calculator {
    constructor() {
        this.outgoings = [];
    }

    addOutgoing(type, amount, occurrence) {
        this.outgoings.push({
            type: type,
            amount: parseFloat(amount),
            occurrence: occurrence
        });
    }

    calculateOutgoings() {
        let groups = {};
        this.outgoings.forEach((bill) => {

            if (!(bill.type in groups)) {
                groups[bill.type] = 0;
            }

            let amount = bill.amount;

            const week = 4.34524,
                fortnight = week / 2,
                every4week = week / 4;

            if (bill.occurrence == 'monthly') {
                bill.amount = bill.amount / week;
            }

            groups[bill.type] += bill.amount;
        });

        return groups;
    }

    calculate() {
        return {
            //incoming: this.calculateIncoming(),
            outgoing: this.calculateOutgoings(),
        }
    }
}