import { Benefit } from "./benefit.model";

export class Employee {
  constructor(public employeeId: number, public name: string, public dob: Date, public salary: number, public benefits: Benefit[]) { }
}
