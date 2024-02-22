import {Component, Self} from '@angular/core';
import {BsDatepickerModule} from "ngx-bootstrap/datepicker";
import {ControlValueAccessor, FormControl, FormsModule, NgControl, ReactiveFormsModule} from "@angular/forms";

@Component({
  selector: 'app-date-picker',
  standalone: true,
  imports: [BsDatepickerModule, ReactiveFormsModule, FormsModule],
  templateUrl: './date-picker.component.html',
  styleUrl: './date-picker.component.css'
})
export class DatePickerComponent implements ControlValueAccessor {

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {
  }

  registerOnChange(fn: any): void {
  }

  registerOnTouched(fn: any): void {
  }

  get control(): FormControl{
    return this.ngControl.control as FormControl;
  }

}
