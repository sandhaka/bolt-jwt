/**
 * Paged users test object
 * @type {{data: {email: string; name: string}[]; page: {size: number; totalElements: number; totalPages: number; pageNumber: number}}}
 */
export const FakeUserPagedData = {
  data: [
    { email: "pippo@pippi.it", username: "pippo11"},
    { email: "pippo@pippi.it", username: "pippo11"},
    { email: "pippo@pippi.it", username: "pippo10"},
    { email: "pippo@pippi.it", username: "pippo0"},
    { email: "pippo@pippi.it", username: "pippo9"},
    { email: "pippo@pippi.it", username: "pippo8"},
    { email: "pippo@pippi.it", username: "pippo7"},
    { email: "pippo@pippi.it", username: "pippo6"},
    { email: "pippo@pippi.it", username: "pippo5"},
    { email: "pippo@pippi.it", username: "pippo4"},
    { email: "pippo@pippi.it", username: "pippo3"},
    { email: "pippo@pippi.it", username: "pippo2"},
    { email: "pippo@pippi.it", username: "pippo1"}
  ],
  page: {
    size: 10,
    totalElements: 13,
    totalPages: 2,
    pageNumber: 1
  }
};
