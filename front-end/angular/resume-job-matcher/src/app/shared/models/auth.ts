export class User {
  public id: string;
  public firstName: string;
  public lastName: string;
  public username: string;
  public email: string;
  public roles: string[];
}

export class Login {
  public emailOrUsername: string;
  public password: string;
  public rememberMe: boolean;
}

export class Authenticated {
  public user: User;
  public token: string;
  public rememberMe: boolean;
}
