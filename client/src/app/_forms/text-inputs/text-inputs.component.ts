import {Component, Input, Self} from '@angular/core';
import {ControlValueAccessor, FormControl, FormsModule, NgControl, ReactiveFormsModule} from "@angular/forms";
import {CommonModule} from "@angular/common";

@Component({
  selector: 'app-text-inputs',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule,CommonModule
  ],
  templateUrl: './text-inputs.component.html',
  styleUrl: './text-inputs.component.css'
})
export class TextInputsComponent implements ControlValueAccessor {

  @Input() label = '';
  @Input() type = 'text';
  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {
  }

  registerOnChange(fn: any): void {
  }

  registerOnTouched(fn: any): void {
  }

  get control(): FormControl {
    return this.ngControl.control as FormControl
  }


}
