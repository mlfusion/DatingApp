export interface Photo {
  push(photo: { id: number; url: string; created: string; description: string; isMain: boolean; }): any;
  id: number;
  url: string;
  description: string;
  dateAdded: string;
  isMain: boolean;

  splice(arg0: any, arg1: number): any;
  findIndex(arg0: (p: any) => boolean): any;
  filter(arg0: (p: any) => boolean): any;

}
