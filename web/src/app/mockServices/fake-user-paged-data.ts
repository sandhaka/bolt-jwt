/**
 * Paged users test object
 */
export const FakeUserPagedData = {
  data: [
    { email: "pippo@pippi.it", username: "pippo11", name: '0', surname: 'test0', id: 0 },
    { email: "pippo@pippi.it", username: "pippo11", name: '1', surname: 'test1', id: 1 },
    { email: "pippo@pippi.it", username: "pippo10", name: '2', surname: 'test2', id: 2 },
    { email: "pippo@pippi.it", username: "pippo0", name: '3', surname: 'test3', id: 3 }
  ],
  page: {
    size: 10,
    totalElements: 13,
    totalPages: 2,
    pageNumber: 1
  }
};
